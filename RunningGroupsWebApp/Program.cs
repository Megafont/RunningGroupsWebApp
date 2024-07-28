using Microsoft.EntityFrameworkCore;

using RunGroupWebApp.Data;
using RunGroupWebApp.Data.Enums;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Repositories;
using RunningGroupsWebApp.Helpers;
using RunningGroupsWebApp.Interfaces;
using RunningGroupsWebApp.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IClubsRepository, ClubsRepository>();
builder.Services.AddScoped<IRacesRepository, RacesRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    Seed.SeedData(app);
}


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

app.Run();
