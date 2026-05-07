namespace apiAlumnos.Data
{
    using Microsoft.EntityFrameworkCore;
    using apiAlumnos.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Alumno> Alumnos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
