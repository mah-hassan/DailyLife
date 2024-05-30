using DailyLife.Api.Contracts;
using DailyLife.Application.Business.Commands.AddPictureToAlbum;
using DailyLife.Application.Business.Commands.Create;
using DailyLife.Application.Business.Commands.Delete;
using DailyLife.Application.Business.Commands.RemovePictureFromAlbum;
using DailyLife.Application.Business.Commands.RemoveProfilePicture;
using DailyLife.Application.Business.Commands.SetProfilePicture;
using DailyLife.Application.Business.Commands.Update;
using DailyLife.Application.Business.Queries.GetBusinesses;
using DailyLife.Application.Business.Queries.GetById;
using DailyLife.Application.Business.Queries.SearchByName;
using DailyLife.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DailyLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BusinessesController : ApiBaseController
{
    public BusinessesController(ISender sender) : base(sender)
    {
    }


    #region Post Endpoints

    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(BusinessRequest request)
    {
        var command = new CreateBusinessCommand(
                     request.name,
                     request.description,
                     request.latituade,
                     request.longituade,
                     request.city,
                     request.state,
                     request.street,
                     request.addressDescription,
                     request.categoryId,
                     request.workTimes);

        return await PrepareResponse(command);
    }

    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpPost("{id}/profilePicture")]
    public async Task<IActionResult> AddProfilePictureAysnc(Guid id, IFormFile picture)
    {
        var command = new SetProfilePictureCommand(id, picture);
        return await PrepareResponse(command);    
    }

    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpPost("{id}/album")]
    public async Task<IActionResult> AddToAlbumAsync(Guid id, List<IFormFile> pictures)
    {
        var command = new AddToAlbumCommand(id, pictures);
        return await PrepareResponse(command);
    }
    #endregion


    #region Get Endpoints
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var query = new GetBusinessByIdQuery(id);
        return await PrepareResponse(query);
    }
    [HttpGet("search")]
    public async Task<IActionResult> GetBusinessesBySearchAsync([FromQuery] string? searchTerm)
    {
        var query = new SearchByNameQuery(searchTerm);
        return await PrepareResponse(query);
    }
    [HttpGet]
    public async Task<IActionResult> GetBusinessesWithPaginationAsync(
        [Range(1, int.MaxValue)]
        int page,
        [Range(1, 100)]
        int size,
        string? fillter,
        decimal? latitude,
        decimal? longitude)
    {
        var query = new GetBusinessesQuery(page, size, fillter, latitude, longitude);
        return await PrepareResponse(query);
    }
    #endregion


    #region Put Endpoints
    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(UpdateBusinessRequest request, Guid id)
    {
        var command = new UpdateBusinessCommand(
            id,
            request.name,
            request.description,
            request.latituade,
            request.longituade,
            request.city,
            request.state,
            request.street,
            request.addressDescription,
            request.workTimes);
       
        return await PrepareResponse(command);
    }
    #endregion


    #region Delete Endpoints

    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var command = new DeleteBusinessCommand(id);
        return await PrepareResponse(command);
    }

    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpDelete("{id}/profilePicture")]
    public async Task<IActionResult> RemoveProfilePictureAsync(Guid id)
    {
        var command = new RemoveProfilePictureCommand(id);
        return await PrepareResponse(command);
    }

    [Authorize(Roles = nameof(AppRoles.BusinessOwner))]
    [HttpDelete("{id}/album/{fileName}")]
    public async Task<IActionResult> RemoveFromAlbumAsync(Guid id, string fileName)
    {
        var command = new RemoveFromAlbumCommand(id, fileName);
        return await PrepareResponse(command);
    }
    #endregion
}
