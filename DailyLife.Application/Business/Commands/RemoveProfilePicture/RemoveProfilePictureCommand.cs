namespace DailyLife.Application.Business.Commands.RemoveProfilePicture;

public sealed record RemoveProfilePictureCommand(Guid businessId)
    : ICommand;
