using MicroservicioProyecto.Application.Services;
using MicroservicioProyecto.Domain.Interfaces;
using MicroservicioProyecto.Domain.Entities;
using MicroservicioProyecto.Infrastructure.Persistence;
using MicroservicioProyecto.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddSingleton<MySqlConnectionSingleton>();

// Repos
builder.Services.AddScoped<IRepository<Proyecto>, ProyectoRepository>();
builder.Services.AddScoped<ProyectoUsuarioRepository>();

// Services
builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<ProyectoUsuarioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
