using System.ComponentModel.DataAnnotations;

namespace VelhosDitados.API.DTOs
{
    public class DitadoCreateDTO
    {
        [Required]
        public string Descricao { get; set; }
    }
}
