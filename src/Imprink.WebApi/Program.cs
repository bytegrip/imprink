using Imprink.WebApi;
using Serilog;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

Startup.ConfigureServices(builder);

var app = builder.Build();

StripeConfiguration.ApiKey = "sk_test_51RaxJBRrcXIyofFGsAGYs1umsvqQVmc6stk3R5lumc1qO2Aq6G0EXgCgDeaJ6aHHJ0pyOz4YDglnceKK7eeNUCOx00VBoIIn2z";

Startup.Configure(app, app.Environment);

app.Run();
