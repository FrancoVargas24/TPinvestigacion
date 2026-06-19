using Entidades;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

using var db= new MusicTradeDbContext();
db.Database.Migrate();

// services SIEMPRE antes de Build
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MusicTradeDbContext>();

var app = builder.Build();


// pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();