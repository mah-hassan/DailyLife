using DailyLife.Domain.Shared;
using MediatR;

namespace DailyLife.Application.Abstractions.Messaging;

internal interface ICommandHandiler<TCommand> 
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}
internal interface ICommandHandiler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}