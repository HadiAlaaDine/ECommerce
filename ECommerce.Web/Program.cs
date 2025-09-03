using ECommerce.Web.Data;
using ECommerce.Web.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
//  Services (DI) configuration
// ---------------------------

// 1) Database (SQL Server) via DefaultConnection from secrets.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 2) Identity (Individual Accounts)
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // للتجربة محليًا بلا تأكيد بريد. رجّعها true لاحقًا إذا بدك.
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 3) MVC Controllers + Views
builder.Services.AddControllersWithViews();

// 4) Bind external service settings from secrets.json
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("Stripe"));

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("Mail"));

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));

var app = builder.Build();

// ---------------------------------
//  HTTP request pipeline (middleware)
// ---------------------------------
if (app.Environment.IsDevelopment())
{
    // صفحة أخطاء EF أثناء التطوير + صفحات الـ Identity (Razor)
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // افتراضيًا 30 يوم
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Routing: MVC + Razor Pages (Identity UI)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// (اختياري) Debug للتأكد إن الأسرار واصلة — احذفهن لاحقًا
// Console.WriteLine("Stripe Key: " + builder.Configuration["Stripe:PublishableKey"]);
// Console.WriteLine("Mail Host: " + builder.Configuration["Mail:Smtp:Host"]);
// Console.WriteLine("Cloudinary Name: " + builder.Configuration["Cloudinary:CloudName"]);

app.Run();