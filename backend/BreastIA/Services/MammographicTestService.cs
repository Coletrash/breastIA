using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BreastIA.Models;
using BreastIA.Data;
using Newtonsoft.Json;
using Azure.Storage.Blobs;

namespace BreastIA.Services
{
    public class MammographicTestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public MammographicTestService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> AnalyzeMammogramAndSaveResultAsync(string imagePath, int patientId, string typeTest)
        {
            var imageUrl = await UploadImageToBlobStorageAsync(imagePath);
            var predictionResult = await AnalyzeImageAsync(imageUrl);

            var mammographicTest = new MammographicTest
            {
                IdPatienst = patientId,
                TypeTest = typeTest,
                Img = imageUrl,
                Result = predictionResult,
                Comments = "No additional comments",
                CreateAt = DateTime.Now
            };

            _context.MammographicTests.Add(mammographicTest);
            await _context.SaveChangesAsync();

            return predictionResult;
        }

        private async Task<string> UploadImageToBlobStorageAsync(string imagePath)
        {
            string connectionString = _configuration["AZURE_STORAGE_CONNECTION_STRING"];
            string containerName = "img"; 
            string blobName = Path.GetFileName(imagePath);

            var blobClient = new BlobServiceClient(connectionString);
            var containerClient = blobClient.GetBlobContainerClient(containerName);
            var blobClientUpload = containerClient.GetBlobClient(blobName);

            using (var fileStream = File.OpenRead(imagePath))
            {
                await blobClientUpload.UploadAsync(fileStream, overwrite: true);
            }

            return $"{_configuration["AZURE_STORAGE_BLOB_URL"]}/{blobName}";
        }

        private async Task<string> AnalyzeImageAsync(string imageUrl)
        {
            string predictionKey = _configuration["PredictionKey"];
            string endpoint = _configuration["Endpoint"];
            string projectId = _configuration["ProjectId"];
            string modelName = _configuration["ModelName"];

            var url = $"{endpoint}/customvision/v3.0/Prediction/{projectId}/classify/iterations/{modelName}/image";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
                client.DefaultRequestHeaders.Add("Content-Type", "application/octet-stream");

                var imageBytes = await DownloadImageAsync(imageUrl);

                var response = await client.PostAsync(url, new ByteArrayContent(imageBytes));
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var predictions = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                var result = predictions?.predictions?[0]?.tagName;

                if (result == null)
                    return "Unknown";

                return result.ToString();
            }
        }

        private async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}
