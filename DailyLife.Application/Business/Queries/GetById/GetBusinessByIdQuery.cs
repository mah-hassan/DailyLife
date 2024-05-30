using DailyLife.Application.Shared.Dtos;

namespace DailyLife.Application.Business.Queries.GetById;

public sealed record GetBusinessByIdQuery(Guid id)
    : IQuery<BusinessDetails>;
