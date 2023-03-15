using APIMe.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using APIMe.Interfaces;
using APIMe.Services;
using APIMe.Utilities.EmailSender;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APIMe.JwtFeatures;
using APIMe.Entities.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.Configure<MailSettings>(options =>
{
    builder.Configuration.GetSection("MailSettings").Bind(options);
});

var connectionString = builder.Configuration.GetConnectionString("APIMeConnection") ?? throw new InvalidOperationException("Connection string 'APIMeConnection' not found.");
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<JwtHandler>();

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

builder.Services.AddTransient<IMailService, MailService>();


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
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();


app.MapFallbackToFile("index.html");

// Run the app
app.Run();

