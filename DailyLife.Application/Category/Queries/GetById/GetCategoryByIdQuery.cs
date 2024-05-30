namespace DailyLife.Application.Category.Queries.GetById;

public sealed record class GetCategoryByIdQuery(Guid id)
    : IQuery<CategoryResponse>;
