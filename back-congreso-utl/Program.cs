// Program.cs - REEMPLAZAR TODO
using Microsoft.EntityFrameworkCore;
using back_congreso_utl.Data;
using back_congreso_utl.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// CONNECTION STRING EN FORMATO ALTERNATIVO
var connectionString = "Host=dpg-d47tmaur433s739nl2i0-a.oregon-postgres.render.com;Database=congresotic_h59v;Username=congreso_user;Password=REpG7CSB7DEYwSqDEdlsgLEyzwu5Wgob;SSL Mode=Require;Trust Server Certificate=true";

builder.Services.AddDbContext<CongresoDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CongresoDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("✅ Database OK - Count: " + db.Participantes.Count());
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ DB Error: " + ex.Message);
    }
}

app.UseCors("AllowAll");

app.MapGet("/api/listado", async (CongresoDbContext db, string? q) =>
{
    if (string.IsNullOrEmpty(q))
        return Results.Ok(await db.Participantes.ToListAsync());

    var searchLower = q.ToLower();
    return Results.Ok(await db.Participantes
        .Where(p => p.Nombre.ToLower().Contains(searchLower) ||
                    p.Apellidos.ToLower().Contains(searchLower) ||
                    p.Ocupacion.ToLower().Contains(searchLower))
        .ToListAsync());
});

app.MapGet("/api/participante/{id}", async (int id, CongresoDbContext db) =>
{
    var p = await db.Participantes.FindAsync(id);
    return p != null ? Results.Ok(p) : Results.NotFound();
});

app.MapPost("/api/registro", async (Participante p, CongresoDbContext db) =>
{
    p.FechaRegistro = DateTime.UtcNow;
    db.Participantes.Add(p);
    await db.SaveChangesAsync();
    return Results.Created($"/api/participante/{p.Id}", p);
});

app.Run();