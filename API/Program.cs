using API.Data;
using API.Data.Seeders;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var JWTSetting = builder.Configuration.GetSection("JWTSetting");
var securityKey = JWTSetting.GetSection("securityKey").Value ?? throw new ArgumentNullException("securityKey", "JWT security key is not configured.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<UserModel, IdentityRole>().AddEntityFrameworkStores<AppDbContext>(). AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>{
   opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
   opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>{
   opt.SaveToken = true;
   opt.RequireHttpsMetadata = false;
   opt.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidAudience = JWTSetting["JWT:ValidAudience"],
       ValidIssuer = JWTSetting["JWT:ValidIssuer"],
       IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(securityKey))
   };
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    // Call the role seeder
    RoleSeeder.SeedRoles(roleManager);
}

app.Run();
