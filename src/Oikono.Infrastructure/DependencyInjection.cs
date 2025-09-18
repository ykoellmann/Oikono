using System.Reflection;
using System.Text;
using Oikono.Application.Common.Events;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Application.Common.Interfaces.Services;
using Oikono.Infrastructure.Cache;
using Oikono.Infrastructure.Cache.CustomCacheAttributes;
using Oikono.Infrastructure.Extensions;
using Oikono.Infrastructure.Persistence;
using Oikono.Infrastructure.Persistence.Interceptors;
using Oikono.Infrastructure.Persistence.Repositories;
using Oikono.Infrastructure.Security;
using Oikono.Infrastructure.Services;
using Oikono.Infrastructure.Settings.Jwt;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Oikono.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddAuth(configuration);

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<OikonoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddHangfire(configuration);

        services.AddRepositories();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
    }

    private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithFluentValidation<JwtSettings>(JwtSettings.SectionName);

        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
        services.AddSingleton<IPasswordHashProvider, PasswordHashProvider>();

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
    }

    private static void AddRepositories(this IServiceCollection collection)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        var repositories = assembly
            .GetTypes()
            .Where(x => x.GetInterface(typeof(IRepository<,>).Name) is not null && x != typeof(Repository<,>))
            .OrderBy(repo => repo.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(field => field.FieldType == typeof(IDistributedCache)))
            .ToList();

        repositories.ForEach(repository =>
        {
            var repositoryInterface =
                repository.GetInterfaces().FirstOrDefault(x => x.Name != typeof(IRepository<,>).Name);

            if (repositoryInterface is null) return;

            if (repository.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(field => field.FieldType == typeof(IDistributedCache)))
            {
                collection.Decorate(repositoryInterface, repository);
            }
            else
            {
                collection.AddTransient(repositoryInterface, repository);
                collection.AddCacheEventHandlers(repositoryInterface, repository);
            }
        });
    }


    private static void AddCacheEventHandlers(this IServiceCollection collection, Type repositoryInterface,
        Type repository)
    {
        var cacheDomainEvents = repository.GetCustomAttributes<CustomClearCacheEventAttribute>()
            .Select(customClearCacheEventAttribute =>
                new CacheDomainEvent(customClearCacheEventAttribute.EventType,
                    typeof(ClearCacheEventHandler<,,,>)))
            .ToList();

        cacheDomainEvents.Add(new CacheDomainEvent(typeof(ClearCacheEvent<,>), typeof(ClearCacheEventHandler<,,,>)));

        var genericEventArguments = repository.GetInterface(typeof(IRepository<,>).Name).GetGenericArguments();

        cacheDomainEvents
            .ForEach(domainEvent =>
            {
                if (domainEvent.EventType.IsGenericType)
                {
                    var arguments = genericEventArguments[Range.EndAt(2)];
                    domainEvent.EventType = domainEvent.EventType.MakeGenericType(arguments);
                }

                domainEvent.EventHandlerType = domainEvent.EventHandlerType.MakeGenericType(repositoryInterface,
                    genericEventArguments[0],
                    genericEventArguments[1],
                    domainEvent.EventType);
            });

        foreach (var domainEvent in cacheDomainEvents)
        {
            var interfaceType = typeof(INotificationHandler<>).MakeGenericType(domainEvent.EventType);
            collection.AddTransient(interfaceType, domainEvent.EventHandlerType);
        }
    }

    private static IServiceCollection AddHttpClient(this IServiceCollection collection)
    {
        collection.AddHttpClient<ExampleHttpService>((provider, client) =>
        {
            client.BaseAddress = new Uri("https://example.com");
            //Add headers, etc.
        });
        collection.AddTransient<IExampleHttpService, ExampleHttpService>();

        return collection;
    }

    private static IServiceCollection AddHangfire(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(configuration.GetConnectionString("DbConnection"));
            });
            config.UseSerilogLogProvider();
            config.UseSerializerSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            });

            config.UseFilter(new QueueFilter());
        });

        collection.AddHangfireServer(provider => { provider.Queues = ["oikono"]; });

        return collection;
    }
}