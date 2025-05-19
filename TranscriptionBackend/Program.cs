using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TranscriptionBackend.Services;
using Microsoft.EntityFrameworkCore;
using TranscriptionBackend.Data;
using TranscriptionBackend.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

//Veritabanı bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//CORS ayarı
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
  {
    policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
  });
});

//JWT ayarları
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is missing.");
var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing.");
var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
      };
    });

//Servisler
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddScoped<TranscriptionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ActivityService>();

var app = builder.Build();

//Seed
using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  DbSeeder.SeedUsers(db);
}

//statik dosyalar için yapılandırma
app.UseStaticFiles(new StaticFileOptions
{
  FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "UploadedFiles")), 
  RequestPath = "/uploads", 
  ServeUnknownFileTypes = true,
  ContentTypeProvider = new FileExtensionContentTypeProvider
  {
    Mappings =
        {
            [".mp3"] = "audio/mpeg"
        }
  }
});

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
