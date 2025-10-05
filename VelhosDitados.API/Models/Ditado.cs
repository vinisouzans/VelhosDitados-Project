using System.ComponentModel.DataAnnotations;

namespace VelhosDitados.API.Models
{
    public class Ditado
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Descricao { get; set; }
    }
}
