using DailyLife.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Category.Commands.Add;

internal sealed class AddCategoryCommandHandiler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandiler<AddCategoryCommand>
{
    public async Task<Result> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var createCategoryResult = CategoryEntity.Create(request.name);
        if (createCategoryResult.IsFailure)
        {
            return createCategoryResult;
        }
        if (await categoryRepository.IsNameExsists(request.name))
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                DomainErrors.CategoryErrors.NameExsists(request.name));
        }
        categoryRepository.Add(createCategoryResult.Value);
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();

    }
}
