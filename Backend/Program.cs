using Microsoft.OpenApi.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();

builder.Services.AddControllers();

// Register Swagger/OpenAPI services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Assignment API", Version = "v1" });
});

builder.Services.AddCors(policy => 
    policy.AddPolicy("AllowAll", opt => 
        opt.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()));

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

app.UseCors("AllowAll"); // Harus sebelum UseRouting / UseEndpoints
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();