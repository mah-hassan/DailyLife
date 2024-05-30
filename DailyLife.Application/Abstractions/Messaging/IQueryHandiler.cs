using DailyLife.Domain.Shared;
using MediatR;

namespace DailyLife.Application.Abstractions.Messaging;

internal interface IQueryHandiler<TQuery, TResponse> :
    IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}