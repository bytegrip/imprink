namespace Imprink.WebApi.Extensions;

public static class StartupEnvironmentExtensions
{
    public static void ConfigureEnvironment(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (app is not WebApplication) return;
        
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }
    }
}