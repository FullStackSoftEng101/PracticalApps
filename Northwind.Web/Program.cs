using Microsoft.AspNetCore.Server.Kestrel.Core; // To use HttpProtocols.
using Northwind.EntityModels;// To use AddNorthwindContext method.

#region Configure the web server host and services.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
    //.AddApplicationPart(typeof(Northwind.WebApi.Controllers.WeatherForecastController).Assembly);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();

var app = builder.Build();
#endregion

#region Configure the HTTP pipeline and routes
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

// 1. ADD THIS HERE: Forces .NET to match URLs to Controllers/Pages BEFORE your custom code runs
app.UseRouting();

// 2. YOUR CUSTOM MIDDLEWARE: Now context.GetEndpoint() will actually work!
app.Use(async (HttpContext context, Func<Task> next) =>
{
    RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;
    if (rep is not null)
    {
        WriteLine($"Endpoint name: {rep.DisplayName}");
        WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
    }
    if (context.Request.Path == "/bonjour")
    {
        await context.Response.WriteAsync("Bonjour Monde!");
        return;
    }
    await next();
});

// 3. MAP ALL ENDPOINTS 
app.MapRazorPages();
app.MapGet("/", () => $"Hello World! {app.Environment.EnvironmentName}");
app.MapControllers(); // This is perfectly fine here now that UseRouting is declared above
#endregion

// Start the web server
app.Run();
WriteLine("This executes after the web server has stopped!");

