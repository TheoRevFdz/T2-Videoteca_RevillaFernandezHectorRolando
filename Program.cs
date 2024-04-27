using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using T2_Videoteca_RevillaFernandezHectorRolando.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<T2_Videoteca_RevillaFernandezHectorRolandoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("T2_Videoteca_RevillaFernandezHectorRolandoContext") ?? throw new InvalidOperationException("Connection string 'T2_Videoteca_RevillaFernandezHectorRolandoContext' not found.")));

// Add services to the container.
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

app.Run();
