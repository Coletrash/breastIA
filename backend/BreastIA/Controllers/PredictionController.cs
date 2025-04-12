using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class PredictionController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public PredictionController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpPost("predict")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> PredictImage(IFormFile file)
    {
        // Claves y configuración
        var predictionKey = "86d134214e17470fa82919baf9ea02cb";
        var endpoint = "https://southcentralus.api.cognitive.microsoft.com";
        var projectId = "bd4600bf-c7f7-4ba1-9fcc-cb67721fd466";  // ID del proyecto
        var modelName = "Iteration5";

        // URL de la API de Custom Vision
        var url = $"{endpoint}/customvision/v3.0/Prediction/{projectId}/detect/iterations/{modelName}/image";

        // Verificar si se recibe el archivo
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // Leer los bytes de la imagen
        byte[] fileBytes;
        using (var memoryStream = new System.IO.MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }

        // Configuración del cliente HttpClient
        var client = _clientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Enviar la imagen a la API de predicción
        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        // Realizar la solicitud POST a la API
        var response = await client.PostAsync(url, byteContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var predictions = JsonConvert.DeserializeObject<PredictionResult>(responseBody);

            // Procesar el resultado y generar el mensaje
            string message = "";

            if (predictions.Predictions[0].TagName == "cancer")
            {
                message = "Breast cancer detected in the mammogram.";
            }
            else if (predictions.Predictions[0].TagName == "tumor")
            {
                message = "An abnormality (tumor) has been detected in the image. Please visit a medical center.";
            }
            else
            {
                message = "No signs of cancer detected.";
            }

            return Ok(new { message });
        }

        return BadRequest("Error al procesar la imagen.");
    }

    public class PredictionResult
    {
        public Prediction[] Predictions { get; set; }
    }

    public class Prediction
    {
        public string TagName { get; set; }
        public float Probability { get; set; }
    }
}
