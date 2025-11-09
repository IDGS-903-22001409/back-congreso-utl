// Program.cs - REEMPLAZAR TODO EL ARCHIVO
using back_congreso_utl.Data;
using back_congreso_utl.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var connectionString = "Host=dpg-d47tmaur433s739nl2i0-a.oregon-postgres.render.com;Database=congresotic_h59v;Username=congreso_user;Password=REpG7CSB7DEYwSqDEdlsgLEyzwu5Wgob;SSL Mode=Require;Trust Server Certificate=true";

builder.Services.AddDbContext<CongresoDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// CRÍTICO: CORS ANTES DE TODO
app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CongresoDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/api/listado", async (CongresoDbContext db, string? q) =>
{
    if (string.IsNullOrEmpty(q))
        return Results.Ok(await db.Participantes.ToListAsync());

    var s = q.ToLower();
    return Results.Ok(await db.Participantes.Where(p =>
        p.Nombre.ToLower().Contains(s) ||
        p.Apellidos.ToLower().Contains(s) ||
        p.Ocupacion.ToLower().Contains(s)).ToListAsync());
}).WithMetadata(new EnableCorsAttribute("AllowAll"));

app.MapGet("/api/participante/{id}", async (int id, CongresoDbContext db) =>
{
    var p = await db.Participantes.FindAsync(id);
    return p != null ? Results.Ok(p) : Results.NotFound();
}).WithMetadata(new EnableCorsAttribute("AllowAll"));

app.MapPost("/api/registro", async (Participante p, CongresoDbContext db) =>
{
    p.FechaRegistro = DateTime.UtcNow;
    db.Participantes.Add(p);
    await db.SaveChangesAsync();
    return Results.Created($"/api/participante/{p.Id}", p);
}).WithMetadata(new EnableCorsAttribute("AllowAll"));

app.Run();