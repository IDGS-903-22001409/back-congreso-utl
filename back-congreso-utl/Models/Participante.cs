using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_congreso_utl.Models
{
    [Table("participantes")]
    public class Participante
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Twitter { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Ocupacion { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public bool AceptaTerminos { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
