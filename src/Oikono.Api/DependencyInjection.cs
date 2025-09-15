using System.Reflection;
using ErrorOr;
using Oikono.Api.Common.Errors;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Infrastructure.Security;
using Oikono.Infrastructure.Security.CurrentUserProvider;
using Oikono.Infrastructure.Security.PolicyEnforcer;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Oikono.Api.Common.Controllers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.MediatR;
using Oikono.Domain.Models;

namespace Oikono.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            option.OperationFilter<IdempotencyRequestHeaderFilter>();
        });

        services.AddSingleton<ProblemDetailsFactory, OikonoProblemDetailsFactory>();

        services.AddSecurity();

        services.AddRateLimiting();

        services.AddMapping();

        services.AddGenericHandlers();

        return services;
    }

    private static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
            {
                options.Window = TimeSpan.FromSeconds(10);
                options.PermitLimit = 5;
            });

            rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
            {
                options.Window = TimeSpan.FromSeconds(15);
                options.SegmentsPerWindow = 3;
                options.PermitLimit = 5;
            });

            rateLimiterOptions.AddTokenBucketLimiter("token", options =>
            {
                options.TokenLimit = 100;
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
                options.TokensPerPeriod = 10;
            });

            rateLimiterOptions.AddConcurrencyLimiter("concurrency", options => { options.PermitLimit = 5; });
        });
    }

    public static void AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    private static void AddSecurity(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevCors", policy =>
            {
                policy.WithOrigins("http://localhost:5173", "")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IPolicyEnforcer, PolicyEnforcer>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
    }

    private static void AddGenericHandlers(this IServiceCollection services)
    {
        var controllerTypes = typeof(Program).Assembly.GetTypes()
            .Where(t => t.BaseType is { IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(Controller<,,,,>));

        foreach (var controller in controllerTypes)
        {
            var args = controller.BaseType!.GetGenericArguments();
            var repo = args[0];
            var entity = args[1];
            var id = args[2];
            var request = args[3];
            var response = args[4];

            var method = typeof(DependencyInjection)
                .GetMethod(nameof(AddCrudHandlers), BindingFlags.Public | BindingFlags.Static)!
                .MakeGenericMethod(repo, entity, id, request, response);

            method.Invoke(null, new object[] { services });
        }
    }

    public static IServiceCollection AddCrudHandlers<TRepo, TEntity, TId, TRequest, TResponse>(
        this IServiceCollection services)
        where TId : Id<TId>, new()
        where TEntity : Entity<TId>
        where TRepo : class, IRepository<TEntity, TId>
    {
        // GetAll
        services.AddTransient<
            IRequestHandler<GetListQuery<TEntity, TId, TResponse>, ErrorOr<List<TResponse>>>,
            GetListQueryHandler<TRepo, TEntity, TId, TResponse>>();

        // GetById
        services.AddTransient<
            IRequestHandler<GetByIdQuery<TEntity, TId, TResponse>, ErrorOr<TResponse>>,
            GetByIdQueryHandler<TRepo, TEntity, TId, TResponse>>();

        // Create
        services.AddTransient<
            IRequestHandler<CreateCommand<TEntity, TId, TRequest, TResponse>, ErrorOr<TResponse>>,
            CreateCommandHandler<TRepo, TEntity, TId, TRequest, TResponse>>();

        // Update
        services.AddTransient<
            IRequestHandler<UpdateCommand<TEntity, TId, TRequest, TResponse>, ErrorOr<TResponse>>,
            UpdateCommandHandler<TRepo, TEntity, TId, TRequest, TResponse>>();

        // Delete
        services.AddTransient<
            IRequestHandler<DeleteCommand<TEntity, TId>, ErrorOr<Deleted>>,
            DeleteCommandHandler<TRepo, TEntity, TId>>();

        return services;
    }
}