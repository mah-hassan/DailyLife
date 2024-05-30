using DailyLife.Application.Helpers;
using DailyLife.Application.Shared.Dtos;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Business.Queries.GetById;

internal sealed class GetBusinessByIdQueryHandiler(IBusinessRepository businessRepository, Mapper mapper)
        : IQueryHandiler<GetBusinessByIdQuery, BusinessDetails>
{
    public async Task<Result<BusinessDetails>> Handle(GetBusinessByIdQuery request, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetById(new Id(request.id));
        if (business is null)
        {
            return Result.Failure<BusinessDetails>(StatusCodes.Status404NotFound,
                DomainErrors.BusinessErrors.NotExsist(request.id));   
        }
        return Result.Success(mapper.ToBusinessDetails(business));
    }
}
