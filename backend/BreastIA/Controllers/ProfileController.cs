using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System;
using BreastIA.Data;
using BreastIA.Models;

namespace BreastIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _blobStorageUrl;
        private readonly string _connectionString;

        public ProfileController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _blobStorageUrl = configuration["AZURE_STORAGE_BLOB_URL"];
            _connectionString = configuration["AZURE_STORAGE_CONNECTION_STRING"];
        }

        // Obtener perfil del usuario
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var user = await _context.Users
                .Where(u => u.IdUsers == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Aplicamos la máscara al correo y devolvemos los valores, incluyendo nulos donde corresponda
            var userProfile = new
            {
                user.IdUsers,
                FullName = string.IsNullOrEmpty(user.FullName) ? null : user.FullName,
            
                Email = MaskEmail(user.Email),
                Country = string.IsNullOrEmpty(user.Country) ? null : user.Country,
                City = string.IsNullOrEmpty(user.City) ? null : user.City,
                Street = string.IsNullOrEmpty(user.Street) ? null : user.Street,
                ImgUsers = string.IsNullOrEmpty(user.ImgUsers) ? null : user.ImgUsers,
                PhoneNumber = user.PhoneNumber == 0 ? null : user.PhoneNumber.ToString(),
            };

            return Ok(userProfile);
        }

        // Método para aplicar la máscara al email
        private string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            var atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                var localPart = email.Substring(0, atIndex);
                var domain = email.Substring(atIndex);
                var maskedLocalPart = localPart[0] + "********" + localPart.Substring(localPart.Length - 4);
                return maskedLocalPart + domain;
            }
            return email;
        }

        // Editar perfil del usuario
        [HttpPost("EditProfile")]
        public async Task<IActionResult> EditProfile(int userId, UserProfileUpdateDto model)
        {
            var user = await _context.Users
                .Where(u => u.IdUsers == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Actualizar los campos con los valores enviados, manteniendo los actuales si son nulos
            user.FullName = model.FullName ?? user.FullName;
         
            user.Country = model.Country ?? user.Country;
            user.City = model.City ?? user.City;
            user.Street = model.Street ?? user.Street;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            // Si se envió una nueva imagen, subimos la imagen a Azure y actualizamos la URL
            if (model.Img != null)
            {
                string imageUrl = await UploadImageToBlobStorage(model.Img);
                user.ImgUsers = imageUrl;
            }

            // Actualizamos los datos en la base de datos
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }
        // Subir imagen al Blob Storage
        private async Task<string> UploadImageToBlobStorage(IFormFile image)
        {
            // Conectamos con Azure Blob Storage
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("img");
            var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(image.FileName));

            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return $"{_blobStorageUrl}/{blobClient.Name}";
        }
    }

    // DTO para actualizar el perfil
    public class UserProfileUpdateDto
    {
        public string FullName { get; set; }
     
        public string Email { get; set; }  // Si es necesario actualizar el email
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public long? PhoneNumber { get; set; }
        public IFormFile Img { get; set; }  // Imagen de perfil (opcional)
    }
}
