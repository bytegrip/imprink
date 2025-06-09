using Imprink.WebApi;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.Configure(app, app.Environment);

app.Run();