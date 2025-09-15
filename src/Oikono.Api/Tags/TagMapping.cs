using Mapster;
using Oikono.Api.Tags.Request;
using Oikono.Api.Tags.Response;
using Oikono.Application.Common.MediatR;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Api.Tags;

internal class TagMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TagRequest, Domain.Recipes.Tag>()
            .MapToConstructor(true);

        config.NewConfig<Domain.Recipes.Tag, TagResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .MapToConstructor(true);

        config.NewConfig<TagRequest, CreateCommand<Domain.Recipes.Tag, TagId, TagRequest, TagResponse>>()
            .MapToConstructor(true);

        config.NewConfig<(TagId, TagRequest), UpdateCommand<Domain.Recipes.Tag, TagId, TagRequest, TagResponse>>()
            .MapToConstructor(true);
    }
}
