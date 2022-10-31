using Microsoft.EntityFrameworkCore;
using MVCUserRoleManager.Areas.Identity.Data;
using MVCUserRoleManager.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MVCUserRoleManagerContextConnection") ?? throw new InvalidOperationException("Connection string 'MVCUserRoleManagerContextConnection' not found.");

// Database context with a connection string to a local database from appsettings.json
builder.Services.AddDbContext<MVCUserRoleManagerContext>(options =>
    options.UseSqlServer(connectionString));

// Adding created User and Role classes and DbContext to Identity.
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<MVCUserRoleManagerContext>();

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
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Adding Razor Pages support, required by default Identity pages.
app.MapRazorPages();

app.Run();