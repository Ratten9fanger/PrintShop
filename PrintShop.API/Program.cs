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
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("./logs/log-.txt")
    .WriteTo.Seq("http://localhost:5341")
    .CreateBootstrapLogger();

try
{
    Log.Information("Приложение запускается...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, lc) => lc
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.File("logs")
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

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
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
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

    builder.Host.UseSerilog();

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение аварийно завершило работу");
}
finally
{
    Log.CloseAndFlush(); // Гарантирует запись всех оставшихся логов
}