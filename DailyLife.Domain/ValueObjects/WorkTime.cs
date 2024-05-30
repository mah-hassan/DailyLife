namespace DailyLife.Domain;

public sealed record WorkTime
{
    public DayOfWeek Day { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}
