using DailyLife.Domain.Errors;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.Shared;

namespace DailyLife.Domain.Entities;

public sealed class Category : Entity
{
    private Category()
        : base(new Id(Guid.NewGuid()))
    {
        
    }
    private Category(Id id, string name) : base(id)
    {
        Name = name;    
    }
    public string Name { get; private set; } = string.Empty;
    public void Update(string name) => Name = name;
    public static Result<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Category>(400,
                new Error("Name",
                "Invalid value: name can not be null or white spaces"));

        return new Category(new Id(Guid.NewGuid()), name);
    }
}
