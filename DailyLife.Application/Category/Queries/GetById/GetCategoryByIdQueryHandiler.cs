using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Category.Queries.GetById;

internal sealed class GetCategoryByIdQueryHandiler
    (ICategoryRepository categoryRepository,
    Mapper mapper)
    : IQueryHandiler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetById(new Id(request.id));
        if (category is null)
        {
            return Result.Failure<CategoryResponse>(StatusCodes.Status400BadRequest,
                DomainErrors.CategoryErrors.NotExsist(request.id));
        }
        return mapper.ToCategoryResponse(category);
    }
}
