// CongresoDbContext.cs - REEMPLAZAR TODO
using Microsoft.EntityFrameworkCore;
using back_congreso_utl.Models;

namespace back_congreso_utl.Data
{
    public class CongresoDbContext : DbContext
    {
        public CongresoDbContext(DbContextOptions<CongresoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Participante> Participantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Participante>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Participante>().HasData(
                new Participante
                {
                    Id = 1,
                    Nombre = "Julián",
                    Apellidos = "Rubio",
                    Email = "jrubio@mail.com",
                    Twitter = "JRubio",
                    Ocupacion = "Desarrollador de Software",
                    Avatar = "👨‍💻",
                    AceptaTerminos = true,
                    FechaRegistro = DateTime.Now
                },
                new Participante
                {
                    Id = 2,
                    Nombre = "Raúl",
                    Apellidos = "Medina",
                    Email = "rmedina@mail.com",
                    Twitter = "RaulMedina",
                    Ocupacion = "Ingeniero Front End",
                    Avatar = "👨‍💼",
                    AceptaTerminos = true,
                    FechaRegistro = DateTime.Now
                },
                new Participante
                {
                    Id = 3,
                    Nombre = "Carlos",
                    Apellidos = "Andrade",
                    Email = "candrade@mail.com",
                    Twitter = "CAndrade",
                    Ocupacion = "Desarrollador Web Full Stack",
                    Avatar = "🧑‍💻",
                    AceptaTerminos = true,
                    FechaRegistro = DateTime.Now
                }
            );
        }
    }
}