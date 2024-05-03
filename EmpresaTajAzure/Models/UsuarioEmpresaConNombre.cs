namespace EmpresaTajAzure.Models
{
    public class UsuarioEmpresaConNombre
    {
        public Usuario Usuario { get; set; }

        public string[] Empresas { get; set; }
    }

    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int? IdClase { get; set; }
        public string Nombre { get; set; }
        public string Role { get; set; }
        public string Linkedin { get; set; }
        public string Email { get; set; }
        public int? Emp_1Id { get; set; }
        public int? Emp_2Id { get; set; }
        public int? Emp_3Id { get; set; }
        public int? Emp_4Id { get; set; }
        public int? Emp_5Id { get; set; }
        public int? Emp_6Id { get; set; }
    }
}
