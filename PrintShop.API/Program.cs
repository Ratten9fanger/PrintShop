using PrintShop.API.Filters;
using PrintShop.Application.Interfaces;
using PrintShop.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers(options => { 
    options.Filters.Add(typeof(GlobalExceptionFilter));
});


builder.Services.AddOpenApi();
                                                                                                    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
