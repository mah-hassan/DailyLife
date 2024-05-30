namespace DailyLife.Application.Favorites.Commands.AddToFavorites;

public sealed record AddToFavoritesCommand(Guid businessId)
    : ICommand;
