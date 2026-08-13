using Microsoft.EntityFrameworkCore;
using PrintShop.API.Extensions;
using PrintShop.Application.Interfaces;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Application.Services;
using PrintShop.DataAccess;
using PrintShop.DataAccess.Repositories;
using PrintShop.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddApiAuthentication(builder.Configuration);


builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddDbContext<PrintShopDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

                                                                                                  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
