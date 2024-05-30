using DailyLife.Domain.Shared;
using MediatR;

namespace DailyLife.Application.Abstractions.Messaging;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
