using DailyLife.Domain.Errors;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.Shared;
using DailyLife.Domain.ValueObjects;

namespace DailyLife.Domain.Entities;

public class Review : Entity
{
    public Comment Comment { get; private set; }
    public int Rate { get; private set; }
    public Guid OwnerId { get; init; }
    public Id BusinessId { get; init; }
    private Review(Id id, Guid ownerId, Id businessId) : base(id)
    {
        OwnerId = ownerId;
        BusinessId = businessId;
    }
    public static Result<Review> Create
        (int rate, 
        Result<Comment> comment,
        Guid ownerId,
        Id businessId)
    {
        var errors = new List<Error>();
        if (rate is <= 0 or > 5)
        {

            errors.Append(new(nameof(rate).ToLower(),
                $"invalid value '{rate}':\nValue must be in range 1 to 5"));
        }
        if (comment.IsFailure)
        {
            errors.AddRange(comment.Errors);
        }
        if(errors.Any())
            return Result.Failure<Review>(400, errors.ToArray());   

        return new Review(new Id(Guid.NewGuid()),
            ownerId,
            businessId)
        {
            Comment = comment.Value,
            Rate = rate
        };
    }  

    public void Update(int rate, Result<Comment> comment)
    {
        if (comment.IsFailure || (rate is < 0 or > 5))
            return;
        
        this.Rate = rate;
        this.Comment = comment.Value;
    }
}
