using DailyLife.Application.Favorites.Commands.AddToFavorites;
using DailyLife.Application.Favorites.Commands.RemoveFromFavorites;
using DailyLife.Application.Favorites.Queries.GetAllFavorites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyLife.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ApiBaseController
    {

        public FavoritesController(ISender sender)
            : base(sender)
        {
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavoritesAsync(Guid businessId)
        {
            var command = new AddToFavoritesCommand(businessId);
            return await PrepareResponse(command);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllFavoritesAsync()
        {
            var query = new GetAllFavoritesQuery();
            return await PrepareResponse(query);
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveFromFavoritesAsync(Guid businessId)
        {
            var command = new RemoveFromFavoritesCommand(businessId);
            return await PrepareResponse(command);
        }
    }
}
