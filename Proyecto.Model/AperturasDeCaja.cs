using System.Security.Principal;

namespace Proyecto.Model
{
    public class AperturasDeCaja
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime FechaDeInicio { get; set; }
        public DateTime? FechaDeCierre { get; set; }
        public string? Observaciones { get; set; }
        public EstadoDeLaCaja Estado { get; set; }
    }
}