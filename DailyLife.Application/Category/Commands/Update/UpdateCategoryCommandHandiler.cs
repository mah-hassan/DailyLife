
using DailyLife.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Category.Commands.Update;

internal sealed class UpdateCategoryCommandHandiler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandiler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetById(new Id(request.id));
        if (category is null)
        {
            return Result.Failure(StatusCodes.Status404NotFound,
                DomainErrors.CategoryErrors.NotExsist(request.id));
        }
        category.Update(request.name);
        categoryRepository.Update(category);
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();    
    }
}