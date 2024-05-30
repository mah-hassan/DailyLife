using DailyLife.Domain.Errors;
using DailyLife.Domain.Shared;

namespace DailyLife.Domain.ValueObjects;
public sealed record Location
{
    public Location(decimal latituade, decimal longituade)
    {
        Latituade = latituade;
        Longituade = longituade;
    }

    public decimal Latituade {  get; private set; }
    public decimal Longituade { get; private set; }
    public static Result<Location> Create(decimal latituade, decimal longituade)
    {
        return new Location(latituade, longituade);
    }
    public Result Update(decimal latituade,decimal longituade)
    {
        Latituade = latituade;
        Longituade = longituade;
        return Result.Success();
    }
}
