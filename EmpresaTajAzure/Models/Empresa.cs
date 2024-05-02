using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaTajAzure.Models
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }

        public string Nombre { get; set; }

        public string Linkedin { get; set; }

        public string Imagen { get; set; }

        public int? Plazas { get; set; }

        public int? PlazasDisponibles { get; set; }
    }
}
