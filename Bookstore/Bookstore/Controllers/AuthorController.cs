using Bookstore.Application.Dtos;
using Bookstore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        public AuthorCreateService AuthorCreateService { get; }
        public AuthorUpdateService AuthorUpdateService { get; }

        public AuthorController(AuthorCreateService authorCreateService,
            AuthorUpdateService authorUpdateService)
        {
            AuthorCreateService = authorCreateService;
            AuthorUpdateService = authorUpdateService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAuthor(AuthorCreate authorCreate)
        {
            var result = await AuthorCreateService.CreateAuthorAsync(authorCreate);
            return Ok(result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAuthor(AuthorUpdate authorUpdate)
        {
            await AuthorUpdateService.UpdateAuthorAsync(authorUpdate);
            return Ok();
        }
    }
}
