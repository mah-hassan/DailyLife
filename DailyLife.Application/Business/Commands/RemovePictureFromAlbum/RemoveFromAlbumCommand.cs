namespace DailyLife.Application.Business.Commands.RemovePictureFromAlbum;

public sealed record RemoveFromAlbumCommand(Guid businessId, string fileName)
    : ICommand;
