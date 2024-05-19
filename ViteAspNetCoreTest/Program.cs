using Asp.Versioning.ApiExplorer;
using System.Text.Json.Serialization;
using Vite.AspNetCore;
using ViteAspNetCoreTest.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddViteServices(options =>
{
    options.Server.AutoRun = true;
    options.Server.Https = true;
    options.Server.PackageDirectory = "ClientApp";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var versionsProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in versionsProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"ViteAspNetCoreTest API - {description.GroupName}");
        }
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.MapRazorPages();

// When page does not exist in Admin area, fallback to Admin/Index
app.MapFallbackToPage("/admin/{*catchall:nonfile}", "/Admin/Index");
// Otherwise fallback to Index but not for API routes
app.MapFallbackToPage("{*path:regex(^(?!api/).*$):nonfile}", "/Index");

// Use the Vite Development Server when the environment is Development.
if (app.Environment.IsDevelopment())
{
    // WebSockets support is required for HMR (hot module reload).
    // Uncomment the following line if your pipeline doesn't contain it.
    app.UseWebSockets();
    // Enable all required features to use the Vite Development Server.
    // Pass true if you want to use the integrated middleware.
    app.UseViteDevelopmentServer(true);
}


app.Run();
