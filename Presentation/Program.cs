using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Presentation.Controllers;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using Presentation.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();

builder.Services.AddScoped<RequireLoginFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<RequireLoginFilter>();  // Register filter globally
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<PollDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PollRepository>();

var jsonFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Json");
var jsonFilePath = Path.Combine(jsonFolderPath, "polls.json"); 

// Inject ILogger for logging
builder.Services.AddSingleton<PollFileRepository>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<PollFileRepository>>(); 
    logger.LogInformation($"JSON Folder Path: {jsonFolderPath}");
    logger.LogInformation($"JSON File Path: {jsonFilePath}");

    if (!Directory.Exists(jsonFolderPath))
    {
        logger.LogInformation("Directory does not exist. Creating directory...");
        Directory.CreateDirectory(jsonFolderPath); 
    }
    else
    {
        logger.LogInformation("Directory already exists.");
    }

    return new PollFileRepository(jsonFilePath, logger); 
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
