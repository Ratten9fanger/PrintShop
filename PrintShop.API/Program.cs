using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using PrintShop.API.Extensions;
using PrintShop.Application.Interfaces;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Application.Services;
using PrintShop.DataAccess;
using PrintShop.DataAccess.Repositories;
using PrintShop.Infrastructure;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//Infrastructure
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

//Services
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();

//Repositories
//builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartRedisRepository, CartRedisRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//Databases
builder.Services.AddDbContext<PrintShopDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
);

Console.WriteLine(builder.Configuration.GetConnectionString("Redis"));

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
