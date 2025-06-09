using Imprink.WebApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq("http://seq:5341",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.Configure(app, app.Environment);

app.Run();