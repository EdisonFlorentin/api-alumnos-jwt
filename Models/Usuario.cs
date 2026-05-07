namespace apiAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string Rol { get; set; } = "User";
    }

}
