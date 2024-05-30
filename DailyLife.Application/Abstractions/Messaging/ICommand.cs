using DailyLife.Domain.Shared;
using MediatR;

namespace DailyLife.Application.Abstractions.Messaging;

internal interface ICommand : IRequest<Result>
{
}
internal interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
