using Microsoft.EntityFrameworkCore;
using TestABPApp.Models.DBModel;
using TestABPApp.Services.Experements;
using TestABPApp.Services.Experements.Imple;
using TestABPApp.Services.Registration;
using TestABPApp.Services.Registration.Imple;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Configs\\experementConfig.json");

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));
// Add services to the container.
builder.Services.AddTransient<IRegistrationDeviceTokenService, RegistrationDeviceTokenService>();
builder.Services.AddTransient<IExperementService, ExperementService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Experement}/{action=ButtonCollor}");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Experement}/{action=Price}");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Experement}/{action=GetExperementStats}");

app.Run();

