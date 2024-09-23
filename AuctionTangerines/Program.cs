using AuctionTangerines.Data;
using AuctionTangerines.Interfaces;
using AuctionTangerines.Models;
using AuctionTangerines.Options;
using AuctionTangerines.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<TangerineGenerateServiceOptions>(options =>
{
    options.TangerineCount = 10; // Количество мандаринок
    options.RunTime = new TimeSpan(17, 27, 0); // Время запуска в 10:00
});

// Добавляем фоновый сервис для генерации мандаринок в определенное время
builder.Services.AddHostedService<TangerineGenerateService>();


builder.Services.Configure<TangerineClearServiceOptions>(options =>
{
    options.RunTime = new TimeSpan(19, 0, 0); // Время запуска в 19:00
});

// Добавляем фоновый сервис для удаления мандаринок в определенное время
builder.Services.AddHostedService<TangerineClearService>();


builder.Services.AddSingleton<IEmailSenderService>(provider => new EmailSenderService(
    smtpServer: "smtp.office365.com",
    smtpPort: 587,
    fromEmail: "vitacoresendler@outlook.com",
    fromPassword: "vc6-2Wa-snF-9PH"
));

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(40);
	options.LoginPath = "/Account/Login";
	options.SlidingExpiration = true;
});

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

app.Run();
