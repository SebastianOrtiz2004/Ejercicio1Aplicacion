using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using PedidoService.Data;
using PedidoService.Middleware;
using PedidoService.Models;
using PedidoService.Repositories;
using PedidoService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger + configuración para JWT en la UI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PedidoService API", Version = "v1" });

    // Definición de esquema de seguridad (Bearer JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT con el prefijo 'Bearer '. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configuración Jwt y EF
builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

builder.Services.AddDbContext<PedidoDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

var jwt = configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new Exception("Sección Jwt no encontrada en appsettings.json");
var key = Encoding.UTF8.GetBytes(jwt.Secret);

// Authentication JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwt.Issuer,
        ValidateAudience = true,
        ValidAudience = jwt.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Middleware propio
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger: solo en desarrollo (pero puedes quitar el if en entornos locales)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PedidoService API V1");
        // c.RoutePrefix = string.Empty; // si quieres que Swagger sea la raíz (https://localhost:5001/)
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
