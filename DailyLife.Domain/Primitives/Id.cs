namespace DailyLife.Domain.Primitives;

public record Id(Guid Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
}
