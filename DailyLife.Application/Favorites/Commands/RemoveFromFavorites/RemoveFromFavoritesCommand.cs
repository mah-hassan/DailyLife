namespace DailyLife.Application.Favorites.Commands.RemoveFromFavorites;

public sealed record RemoveFromFavoritesCommand(Guid businessId)
    : ICommand;
