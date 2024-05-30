using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using DailyLife.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Favorites.Commands.AddToFavorites;

internal sealed class AddToFavoritesCommandHandiler(
    IFavoritesRepository favoritesRepository,
    IBusinessRepository businessRepository,
    IHttpContextAccessor contextAccessor,
    IUnitOfWork unitOfWork)
        : ICommandHandiler<AddToFavoritesCommand>
{

    public async Task<Result> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetById(new Id(request.businessId));
        if (business is null)
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                DomainErrors.BusinessErrors.NotExsist(request.businessId));   
        }

        var userId = contextAccessor.GetUserId();

        var favorite = await favoritesRepository.GetFavorite(userId, business.Id);

        if (favorite is {})
        {
            return Result.Failure(StatusCodes.Status400BadRequest,
                new Error("Favorites", $"{business.Name} already exsist in user favorites"));
        }
        favoritesRepository.AddFavoriteBusiness(new FavoriteBusiness(business.Id, userId));
        await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        return Result.Success();
    }
}