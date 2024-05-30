namespace DailyLife.Application.Shared.Dtos;

public sealed class PagenationResponse<TResponse>
     where TResponse : class
{
    public int CurrentPage { get; set; }
    public bool HasNextPage { get; set; }
    public int TotalCount { get; set; }
    public List<TResponse> Result { get; set; } = new();

}