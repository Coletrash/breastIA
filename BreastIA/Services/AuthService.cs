using BreastIA.Models;
using BreastIA.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace BreastIA.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null) return false;

                // Generar el hash de la contraseña proporcionada
                using var hmac = new HMACSHA256();
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Comparar el hash de la contraseña proporcionada con el hash almacenado
                return computedHash.SequenceEqual(user.Password);
            }
            catch (Exception ex)
            {
                // Registrar el error para diagnóstico
                Console.WriteLine($"Error al validar el usuario: {ex.Message}");
                return false;
            }
        }
    }
}
