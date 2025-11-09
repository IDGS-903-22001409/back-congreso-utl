using Microsoft.EntityFrameworkCore;
using back_congreso_utl.Data;
using back_congreso_utl.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS CONFIGURATION
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// DATABASE CONFIGURATION
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {(string.IsNullOrEmpty(connectionString) ? "EMPTY!" : "Set")}");

builder.Services.AddDbContext<CongresoDbContext>(options =>
    options.UseNpgsql("postgres://congreso_user:REpG7CSB7DEYwSqDEdlsgLEyzwu5Wgob@dpg-d47tmaur433s739nl2i0-a.oregon-postgres.render.com/congresotic_h59v"));

var app = builder.Build();

// DATABASE AUTO-CREATION
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CongresoDbContext>();
    try
    {
        Console.WriteLine("Creating database...");
        db.Database.EnsureCreated();
        Console.WriteLine("Database created successfully!");

        var count = db.Participantes.Count();
        Console.WriteLine($"Participantes in database: {count}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR creating database: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
    }
}

// SWAGGER (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANTE: CORS DEBE IR ANTES DE LOS ENDPOINTS
app.UseCors("AllowAll");

// ENDPOINTS
app.MapGet("/api/listado", async (CongresoDbContext db, string? q) =>
{
    try
    {
        if (string.IsNullOrEmpty(q))
        {
            var all = await db.Participantes.ToListAsync();
            return Results.Ok(all);
        }

        var searchLower = q.ToLower();
        var results = await db.Participantes
            .Where(p => p.Nombre.ToLower().Contains(searchLower) ||
                        p.Apellidos.ToLower().Contains(searchLower) ||
                        p.Ocupacion.ToLower().Contains(searchLower))
            .ToListAsync();

        return Results.Ok(results);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in listado: {ex.Message}");
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/participante/{id}", async (int id, CongresoDbContext db) =>
{
    try
    {
        var participante = await db.Participantes.FindAsync(id);
        return participante is not null ? Results.Ok(participante) : Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting participante: {ex.Message}");
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/registro", async (Participante participante, CongresoDbContext db) =>
{
    try
    {
        Console.WriteLine($"Registering: {participante.Nombre} {participante.Apellidos}");

        participante.FechaRegistro = DateTime.UtcNow;
        db.Participantes.Add(participante);
        await db.SaveChangesAsync();

        Console.WriteLine($"Registered successfully with ID: {participante.Id}");
        return Results.Created($"/api/participante/{participante.Id}", participante);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error registering: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        return Results.Problem(ex.Message);
    }
});

Console.WriteLine("API started successfully!");
app.Run();