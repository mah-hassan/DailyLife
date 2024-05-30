using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Entities;

public class FavoriteBusiness
{
    public FavoriteBusiness(Id businessId, Guid userId)
    {
        BusinessId = businessId;
        UserId = userId;
    }

    private FavoriteBusiness()
    {
            
    }
    public Id BusinessId { get; init; }
    public Guid UserId { get; init; }
}