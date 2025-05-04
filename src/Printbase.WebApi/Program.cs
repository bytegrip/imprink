using Printbase.WebApi;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.Configure(app, app.Environment);

app.Run();