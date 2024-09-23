using Features.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddExceptionHandlers()
    .AddAppServices()
    .AddDynamicInfrastructureServices(builder.Configuration)
    .AddMiddlewares()
    .AddMediator()
    .AddAutoMapper()
    .AddBotRequests()
    .AddControllers()
    .AddNewtonsoftJson(options => options.UseMemberCasing());

await builder.Services.SetTelegramBotWebHook(builder.Configuration);

var app = builder.Build();

app
    .UseHsts()
    .UseRouting()
    .UseCustomRequestLocalization()
    .UseExceptionHandler()
    .UseMiddlewares();
app.MapControllers();

var classesDbInitializer = app.Services.GetRequiredService<IClassesDbInitializer>();
await classesDbInitializer.MigrateDbAsync(app);

app.Run();