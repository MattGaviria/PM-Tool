using Lab1.Areas.ProjectManagement.Models;
using Lab1.Data;
using Lab1.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add the context tp tje service collection with a connection string
builder.Services.AddDbContext<ApplicationDbContext>( options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


//Can Also be configured on appsettings.jason
/*
"Serilog" : {
    "MinimumLevel": {
        "Default": "Debug",
        "Override" : {
            "Microsoft": "Warning",
            "System": "Warning"
        }
    },
    
    "WriteTo": [
    {"Name" :  "Console"},
    {
        "Name:" : "File",
        "Args" : {
            "path" : "Logs/log-.txt",
            "rollingInterval" : "Day"
        }
    }
    ],
    "Enrich" : ["FromLogContext"]
}
*/



//Logging level: Verbose, debug, information, warning, Error, fatal  
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

//Inject SendGrid Email Sender
builder.Services.AddSingleton<IEmailSender, EmailSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapStaticAssets();


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Project}/{action=Index}/{id?}"
);





app.Run();