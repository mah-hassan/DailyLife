
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Category.Queries.GetAllCategories;

internal sealed class GetAllCategoriesQueryHandiler(
    ICategoryRepository categoryRepository,
    Mapper mapper)
    : IQueryHandiler<GetAllCategoriesQuery, List<CategoryResponse>>
{
 
    public async Task<Result<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);

        if (!categories.Any())
            return Result.Failure<List<CategoryResponse>>(StatusCodes.Status204NoContent,
            new Error("result", "No categories was found"));

        return Result
            .Success(categories.Select(c => mapper.ToCategoryResponse(c)).ToList());

    }
}
