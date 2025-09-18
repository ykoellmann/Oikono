using ErrorOr;
using MapsterMapper;
using MediatR;
using Oikono.Application.Common.Interfaces.MediatR.Handlers;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Persistence.Recipes;
using Oikono.Application.Recipes.Common;
using Oikono.Domain.Errors;
using Oikono.Domain.Recipes;
using Oikono.Domain.Recipes.ValueObjects;

namespace Oikono.Application.Recipes.Commands;

public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, RecipeDetailResult>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly ISideDishRepository _sideDishRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IPartRepository _partRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRecipeCommandHandler(IRecipeRepository recipeRepository, ISideDishRepository sideDishRepository,
        ITagRepository tagRepository, IPartRepository partRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _recipeRepository = recipeRepository;
        _sideDishRepository = sideDishRepository;
        _tagRepository = tagRepository;
        _partRepository = partRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<RecipeDetailResult>> Handle(CreateRecipeCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var recipe = new Recipe(request.Name, request.Portions, request.Calories, request.Rating);
            recipe = await _recipeRepository.AddAsync(recipe, request.UserId, cancellationToken);

            foreach (SideDishId sideDishId in request.SideDishes)
            {
                var sideDish = await _sideDishRepository.GetByIdAsync(sideDishId, cancellationToken);
                if (sideDish == null)
                    continue;
                recipe.AddSideDish(sideDish);
            }

            foreach (TagId tagId in request.Tags)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId, cancellationToken);
                if (tag == null)
                    continue;
                recipe.AddTag(tag);
            }

            foreach (var partRequest in request.Parts)
            {
                var part = new Part(recipe.Id, partRequest.Name);
                part = await _partRepository.AddAsync(part, request.UserId, cancellationToken);
                foreach (var partIngredientRequest in partRequest.Ingredients)
                {
                    var partIngredient = new PartIngredient(part.Id, (IngredientId)partIngredientRequest.IngredientId,
                        partIngredientRequest.Amount, partIngredientRequest.Unit);
                    part.AddIngredient(partIngredient);
                }

                await _partRepository.UpdateAsync(part, request.UserId, cancellationToken);
                recipe.AddPart(part);
            }

            foreach (var stepRequest in request.Steps)
            {
                var step = new Step(recipe.Id, stepRequest.Description, stepRequest.Duration, (DeviceId)stepRequest.DeviceId,
                    stepRequest.Temperature);
            
                recipe.AddStep(step);
            }
        
            await _recipeRepository.UpdateAsync(recipe, request.UserId, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            recipe = await _recipeRepository.GetByIdAsync(recipe.Id, cancellationToken);
        
            return _mapper.Map<RecipeDetailResult>(recipe);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Errors.Recipe.Creation;
        }
    }
}