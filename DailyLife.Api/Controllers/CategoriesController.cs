using DailyLife.Application.Category.Commands.Add;
using DailyLife.Application.Category.Commands.Delete;
using DailyLife.Application.Category.Commands.Update;
using DailyLife.Application.Category.Queries.GetAllCategories;
using DailyLife.Application.Category.Queries.GetById;
using DailyLife.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ApiBaseController
{
    public CategoriesController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = nameof(AppRoles.Admin))]
    [HttpPost]
    public async Task<IActionResult> AddAsync(string name)
    {
        var command = new AddCategoryCommand(name);
        return await PrepareResponse(command);
    }



    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var query = new GetCategoryByIdQuery(id);
        return await PrepareResponse(query);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetAllCategoriesQuery();
        return await PrepareResponse(query);
    }

    [Authorize(Roles = nameof(AppRoles.Admin))]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, string name)
    {
        var command = new UpdateCategoryCommand(id, name);
        return await PrepareResponse(command);
    }

    [Authorize(Roles = nameof(AppRoles.Admin))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        return await PrepareResponse(command);
    }
}