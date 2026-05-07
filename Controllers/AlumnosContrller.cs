namespace apiAlumnos.Controllers
{
    using apiAlumnos.Models;
    using Microsoft.AspNetCore.Mvc;
    using apiAlumnos.Data;
    using Microsoft.AspNetCore.Authorization;


    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlumnosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlumnosController(AppDbContext context)
        {
            _context = context;
        }

        static List<Alumno> alumnos = new List<Alumno>()
        {
        new Alumno { Id = 1, Nombre = "Edison", Edad = 22 }
        };

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult Get()
        {
            var alumnos = _context.Alumnos.ToList();
            return Ok(alumnos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post(Alumno alumno)
        {
            if (string.IsNullOrWhiteSpace(alumno.Nombre))
                return BadRequest("El nombre es obligatorio");
            _context.Alumnos.Add(alumno);
            _context.SaveChanges();
            return Ok(alumno);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, Alumno alumno)
        {
            if (id != alumno.Id)
                return BadRequest("El ID no coincide");

            var alumnoExistente = _context.Alumnos.Find(id);

            if (alumnoExistente == null)
                return NotFound();

            alumnoExistente.Nombre = alumno.Nombre;
            alumnoExistente.Edad = alumno.Edad;
            alumnoExistente.Curso = alumno.Curso;

            _context.SaveChanges();

            return Ok(alumnoExistente);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var alumno = _context.Alumnos.Find(id);

            if (alumno == null)
                return NotFound();

            _context.Alumnos.Remove(alumno);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var alumno = _context.Alumnos.Find(id);

            if (alumno == null)
                return NotFound();

            return Ok(alumno);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("buscar")]
        public IActionResult Buscar(string nombre)
        {
            var alumnos = _context.Alumnos
                .Where(a => a.Nombre.Contains(nombre))
                .ToList();

            return Ok(alumnos);
        }

    }
}
