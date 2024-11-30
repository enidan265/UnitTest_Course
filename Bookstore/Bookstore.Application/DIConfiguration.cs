using Bookstore.Application.Services;
using Bookstore.Application.Validation;
using Bookstore.Domain.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Application
{
    public static class DIConfiguration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<BookCreateService>();
            services.AddScoped<BookUpdateService>();
            services.AddScoped<BookDeliveryService>();
            services.AddScoped<BookSaleService>();
            services.AddScoped<BookFilterService>();
            services.AddScoped<AuthorCreateService>();
            services.AddScoped<AuthorUpdateService>();
            services.AddScoped<BookCreateValidator>();
            services.AddScoped<BookUpdateValidator>();
            services.AddScoped<BookSaleValidator>();
            services.AddScoped<BookDeliveryValidator>();
            services.AddScoped<AuthorCreateValidator>();
            services.AddScoped<AuthorUpdateValidator>();
            services.AddScoped<BookValidator>();
            services.AddAutoMapper(typeof(DtoEntityMapperProfile));
        }
    }
}
