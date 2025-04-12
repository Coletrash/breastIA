using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreastIA.Models;
using BreastIA.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BreastIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Doctor
        [HttpPost]
        [Authorize] // Asegura que solo usuarios autenticados puedan acceder
        public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Invalid data.");
            }

            // Obtener el ID del usuario desde el token
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            doctor.IdUserDoctor = userId.Value;

            // Si se proporciona un IdHospitalDoctor, verificar que exista
            if (doctor.IdHospitalDoctor.HasValue)
            {
                var hospital = await _context.HospitalDoctor.FindAsync(doctor.IdHospitalDoctor);
                if (hospital == null)
                {
                    return NotFound("Hospital not found.");
                }
            }

            // Guardar el doctor
            _context.Doctor.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.IdDoctor }, doctor);
        }

        // Método para obtener el ID del usuario desde el token
        private int? GetUserIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return null;
            }

            return userId;
        }

        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctor
                .Include(d => d.HospitalDoctor)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.IdDoctor == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }
    }
}
