using Bookstore.Application.Dtos;
using Bookstore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        public BookCreateService BookCreateService { get; }
        public BookUpdateService BookUpdateService { get; }
        public BookSaleService BookSaleService { get; }
        public BookDeliveryService BookDeliveryService { get; }
        public BookFilterService BookFilterService { get; }

        public BookController(BookCreateService bookCreateService,
            BookUpdateService bookUpdateService,
            BookSaleService bookSaleService,
            BookDeliveryService bookDeliveryService,
            BookFilterService bookFilterService)
        {
            BookCreateService = bookCreateService;
            BookUpdateService = bookUpdateService;
            BookSaleService = bookSaleService;
            BookDeliveryService = bookDeliveryService;
            BookFilterService = bookFilterService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateBook(BookCreate bookCreate)
        {
            var result = await BookCreateService.CreateBookAsync(bookCreate);
            return Ok(result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateBook(BookUpdate bookUpdate)
        {
            await BookUpdateService.UpdateBookAsync(bookUpdate);
            return Ok();
        }

        [HttpPost]
        [Route("Delivery")]
        public async Task<IActionResult> BookDelivery(BookDelivery bookDelivery)
        {
            await BookDeliveryService.ProcessBookDeliveryAsync(bookDelivery);
            return Ok();
        }

        [HttpPost]
        [Route("Sale")]
        public async Task<IActionResult> BookSale(BookSale bookSale)
        {
            await BookSaleService.ProcessBookSaleAsync(bookSale);
            return Ok();
        }

        [HttpGet]
        [Route("Filter")]
        public async Task<IActionResult> GetFilteredBooks(BookFilter filter)
        {
            var result = await BookFilterService.GetFilteredBooksAsync(filter);
            return Ok(result);
        }

    }
}
