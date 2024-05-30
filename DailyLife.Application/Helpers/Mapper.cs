using DailyLife.Application.Abstractions;
using DailyLife.Application.Category.Queries;
using DailyLife.Application.Shared.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace DailyLife.Application.Helpers;

internal class Mapper
{
    private readonly IServiceScopeFactory  _serviceScopeFactory;

    private readonly IFileService _fileService; 
    public Mapper(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        using var scope = _serviceScopeFactory.CreateScope();
        _fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
    }

    public CategoryResponse ToCategoryResponse(CategoryEntity category)
        => new CategoryResponse(category.Id.Value, category.Name);

    public BusinessDetails ToBusinessDetails(BusinessAggregate business)
    {
        if (business is null)
        {
            throw new InvalidOperationException("can not map null business");
        }
        
        return new BusinessDetails(
            business.Id.Value,
            business.Name,
            business.Description,
            _fileService.GetAbsolutePath(business.ProfilePicture),
            Directory.GetFiles(business.Album!)
            .Select(p => _fileService.GetAbsolutePath(p)).ToArray(),
            business.Category.Name,
            business.Location,
            business.Address,
            business.WorkTimes);
    }
}
