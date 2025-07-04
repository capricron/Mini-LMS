using Microsoft.OpenApi.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Repository;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // <-- Ini penting!
builder.Services.AddScoped<ILearnerService, LearnerService>(); // <-- Ini penting!
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILearnerRepository, LearnerRepository>(); // <-- BARU
builder.Services.AddScoped<ILearnerService, LearnerService>();


builder.Services.AddControllers();

// Register Swagger/OpenAPI services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Assignment API", Version = "v1" });

    // Tambahkan konfigurasi security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header menggunakan Bearer. Contoh: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.xxxxx'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Tambahkan security requirement agar semua endpoint memerlukan auth
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddCors(policy =>
    policy.AddPolicy("AllowAll", opt =>
        opt.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assignment API v1");
        c.RoutePrefix = "swagger"; // Untuk akses via /swagger
    });
}

// Middleware penting
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

// Harus sebelum MapControllers()
app.UseAuthentication(); // Membaca token dari header
app.UseAuthorization(); // Memvalidasi token

app.Use(async (context, next) =>
{
    var user = context.User;
    Console.WriteLine($"User ID: {user.Identity?.Name}, Roles: {string.Join(", ", user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))}");
    await next();
});

app.MapControllers();

app.Run();