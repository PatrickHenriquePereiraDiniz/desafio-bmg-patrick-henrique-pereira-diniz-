using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DesafioBMG.Data;
using DesafioBMG.Repositories;
using DesafioBMG.Services;
using DesafioBMG.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext em memória
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("BMGDb"));

// Repositories
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Configuração JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
    throw new Exception("JWT Key não encontrada no appsettings.json!");

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BMG E-Commerce API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token JWT}"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User
            {
                Id = Guid.Parse("2085b20e-9e5c-4035-a50a-c4a8e8320fdd"),
                Email = "admin@bmg.com",
                Password = "admin123",
                Role = "Admin"
            },
            new User
            {
                Id = Guid.Parse("67f4d591-5833-4358-9e98-feaac1592656"),
                Email = "cliente@bmg.com",
                Password = "cliente123",
                Role = "Customer"
            }
        );
        context.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
