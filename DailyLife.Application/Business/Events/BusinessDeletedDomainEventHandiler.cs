using DailyLife.Application.Abstractions;
using DailyLife.Domain.DomainEvents;
using MediatR;

namespace DailyLife.Application.Business.Events;

internal sealed class BusinessDeletedDomainEventHandiler(
        IFavoritesRepository favoritesRepository,
        IUnitOfWork unitOfWork)
    : INotificationHandler<BusinessDeletedDomainEvent>
{
    public async Task Handle(BusinessDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var favorites = await favoritesRepository
                                .GetAllFavoritesForAspecificBusiness(notification.businessId, cancellationToken);
        if(favorites.Any())
        {
            foreach(var favorite in favorites)
                favoritesRepository.RemoveFromFavorites(favorite);
            
            await unitOfWork.SaveBusinessChangesAsync(cancellationToken);
        }
    }
}
