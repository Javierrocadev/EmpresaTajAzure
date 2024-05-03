using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaTajAzure.Models
{
    public class EntrevistaAlumno
    {
        public int IdentEntrevista { get; set; }

        public int IdAlumno { get; set; }

        public int IdEmpresa { get; set; }

        public DateTime FechaEntrevista { get; set; }

        public string Estado { get; set; }

        public Usuario Alumno { get; set; }

        public Empresa Empresa { get; set; }
    }
}
