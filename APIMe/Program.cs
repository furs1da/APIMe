using APIMe.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using APIMe.Interfaces;
using APIMe.Utilities.EmailSender;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APIMe.JwtFeatures;
using APIMe.Entities.Models;
using APIMe.Entities.Configuration;
using APIMe.Services.Email;
using APIMe.Services.Routes;
using APIMe.Mapping;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configure services


var connectionString = builder.Configuration.GetConnectionString("APIMeConnection") ?? throw new InvalidOperationException("Connection string 'APIMeConnection' not found.");
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<JwtHandler>();

var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<RouteService>();
builder.Services.AddScoped<RouteLogService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();


builder.Services.AddDbContext<APIMeContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;


    options.User.RequireUniqueEmail = true;
    //options.SignIn.RequireConfirmedEmail = true;

})
    .AddEntityFrameworkStores<APIMeContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



// Configure the app
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy"); // Add this line
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

APIMeContext.CreateAdminUser(app.Services).Wait();

app.MapFallbackToFile("index.html");

// Run the app
app.Run();
