using Microsoft.AspNetCore.Mvc;
using BreastIA.Data;
using BreastIA.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BreastIA.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRegistrationRequest request)
        {
            // Validar formato del email
            if (!IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email format.");
            }

            // Validar que el password y confirm password coincidan
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password do not match.");
            }

            // Verificar si el email ya existe
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Email already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password, // Texto plano para pruebas
                FullName = request.FullName,
             
                Street = request.Street,
                
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Patient registered successfully." });
        }

  

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }

    // DTO para el registro de pacientes
    public class PatientRegistrationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }

        //public string ImgUsers  {get; set; }
}

   
}
