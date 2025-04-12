using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using BreastIA.Data;
using BreastIA.DTOs;
using BreastIA.Models;

namespace BreastIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor
        public RemindersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener recordatorios vigentes del usuario
        [HttpGet("GetReminders")]
        public async Task<IActionResult> GetReminders(int userId)
        {
            // Verificar si el usuario existe
            var userExists = await _context.Users.AnyAsync(u => u.IdUsers == userId);
            if (!userExists)
            {
                return NotFound(new { message = "User not found" });
            }

            // Obtener recordatorios vigentes (comparar solo la fecha sin tener en cuenta la hora)
            var reminders = await _context.Reminders
                .Where(r => r.IdUserReminders == userId && r.DateReminders.Date >= DateTime.Now.Date) // Comparar solo la fecha
                .Select(r => new { r.Title, r.DateReminders })
                .ToListAsync();

            if (reminders == null || !reminders.Any())
            {
                return Ok(new { message = "No active reminders" });
            }

            return Ok(reminders);
        }
    }
}
