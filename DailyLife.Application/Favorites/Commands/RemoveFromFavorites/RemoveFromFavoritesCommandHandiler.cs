using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Favorites.Commands.RemoveFromFavorites;

internal sealed class RemoveFromFavoritesCommandHandiler(
        IFavoritesRepository favoritesRepository,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor contextAccessor,
        IBusinessRepository businessRepository)
    : ICommandHandiler<RemoveFromFavoritesCommand>
{
    public async Task<Result> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetById(new Id(request.businessId));
        if (business is null)
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                    DomainErrors.BusinessErrors.NotExsist(request.businessId));
        }
        var userId = contextAccessor.GetUserId();
        var favorite = await favoritesRepository.GetFavorite(userId, business.Id);
        if (favorite is null)
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                new Error("Favorites", $"business does not exsist in user favorites"));
        }
        favoritesRepository.RemoveFromFavorites(favorite);
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}
