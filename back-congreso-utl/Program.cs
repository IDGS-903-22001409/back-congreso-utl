using Microsoft.EntityFrameworkCore;
using back_congreso_utl.Data;
using back_congreso_utl.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Swagger para debug
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 CORS: permitir desde cualquier origen (Render + localhost)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 🔹 Conexión a PostgreSQL en Render
var connectionString = "Host=dpg-d47tmaur433s739nl2i0-a.oregon-postgres.render.com;Database=congresotic_h59v;Username=congreso_user;Password=REpG7CSB7DEYwSqDEdlsgLEyzwu5Wgob;SSL Mode=Require;Trust Server Certificate=true";

builder.Services.AddDbContext<CongresoDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// ✅ Aplicar CORS ANTES de los endpoints
app.UseCors("AllowAll");

// ✅ (Opcional) Swagger solo para debug
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Inicializar base de datos (sin EnsureCreated si ya existe)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CongresoDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine($"✅ DB conectada - Participantes: {db.Participantes.Count()}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error DB: {ex.Message}");
    }
}

// 🔹 Endpoints API
app.MapGet("/api/listado", async (CongresoDbContext db, string? q) =>
{
    if (string.IsNullOrEmpty(q))
        return Results.Ok(await db.Participantes.ToListAsync());

    var search = q.ToLower();
    return Results.Ok(await db.Participantes
        .Where(p => p.Nombre.ToLower().Contains(search)
                 || p.Apellidos.ToLower().Contains(search)
                 || p.Ocupacion.ToLower().Contains(search))
        .ToListAsync());
});

app.MapGet("/api/participante/{id}", async (int id, CongresoDbContext db) =>
{
    var p = await db.Participantes.FindAsync(id);
    return p != null ? Results.Ok(p) : Results.NotFound();
});

app.MapPost("/api/registro", async (Participante p, CongresoDbContext db) =>
{
    try
    {
        p.FechaRegistro = DateTime.Now;
        db.Participantes.Add(p);
        await db.SaveChangesAsync();
        return Results.Created($"/api/participante/{p.Id}", p);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error en POST /registro: {ex.Message}");
        return Results.Problem("Error al registrar participante: " + ex.Message);
    }
});

app.Run();
