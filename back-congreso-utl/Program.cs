// Program.cs - Versión SIN WithOpenApi
using Microsoft.EntityFrameworkCore;
using back_congreso_utl.Data;
using back_congreso_utl.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<CongresoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.MapGet("/api/listado", async (CongresoDbContext db, string? q) =>
{
    if (string.IsNullOrEmpty(q))
    {
        return Results.Ok(await db.Participantes.ToListAsync());
    }

    var searchLower = q.ToLower();
    var results = await db.Participantes
        .Where(p => p.Nombre.ToLower().Contains(searchLower) ||
                    p.Apellidos.ToLower().Contains(searchLower) ||
                    p.Ocupacion.ToLower().Contains(searchLower))
        .ToListAsync();

    return Results.Ok(results);
});

app.MapGet("/api/participante/{id}", async (int id, CongresoDbContext db) =>
{
    var participante = await db.Participantes.FindAsync(id);
    return participante is not null ? Results.Ok(participante) : Results.NotFound();
});

app.MapPost("/api/registro", async (Participante participante, CongresoDbContext db) =>
{
    db.Participantes.Add(participante);
    await db.SaveChangesAsync();
    return Results.Created($"/api/participante/{participante.Id}", participante);
});

app.Run();