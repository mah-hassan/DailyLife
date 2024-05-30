
using DailyLife.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Category.Commands.Delete;

internal sealed class DeleteCategoryCommandHandiler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandiler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetById(new Id(request.id));
        if (category is null)
        {
            return Result.Failure(StatusCodes.Status404NotFound,
                DomainErrors.CategoryErrors.NotExsist(request.id));
        }
        categoryRepository.Delete(category);    
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}
