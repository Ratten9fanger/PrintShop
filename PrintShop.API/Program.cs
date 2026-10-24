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
using Confluent.Kafka;

// Bootstrap-logger живёт ДО создания builder, минимальная консоль ему хватит
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Приложение запускается...");

    var builder = WebApplication.CreateBuilder(args);

    // ЕДИНСТВЕННАЯ точка настройки Serilog: всё читается из appsettings.json
    builder.Host.UseSerilog((context, services, lc) => lc
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();

    //Infrastructure
    builder.Services.AddApiAuthentication(builder.Configuration);
    builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
    builder.Services.AddScoped<IPdfHelper, PdfHelper>();
    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

    builder.Services.AddSingleton<IProducer<string, string>>(_ =>
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            Acks = Acks.All,
            EnableIdempotence = true
        };

        return new ProducerBuilder<string, string>(config).Build();
    });

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

    Log.Warning(builder.Configuration.GetConnectionString("Redis")!);

    //Extensions
    builder.Services.AddHttpClient("NotificationService", client =>
    {
        client.BaseAddress = new Uri("https://localhost:34555");
        client.Timeout = TimeSpan.FromSeconds(5);
    });

    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();   // правка: одна строка лога на запрос
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health");   // правка 

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