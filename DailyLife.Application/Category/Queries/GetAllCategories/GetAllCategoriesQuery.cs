namespace DailyLife.Application.Category.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery()
    : IQuery<List<CategoryResponse>>;
