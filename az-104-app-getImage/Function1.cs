using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;

namespace az_104_app_getImage
{
    public static class Function1
    {
        [FunctionName("GetImage")]
        public static async Task<IActionResult> GetImage(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // �N�G���p�����[�^����blobName���擾
                string blobName = req.Query["blobName"];
                if (string.IsNullOrEmpty(blobName))
                {
                    return new BadRequestObjectResult("Please provide a valid blobName.");
                }

                // ���ϐ�����X�g���[�W�ڑ�������ƃR���e�i�����擾
                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                string containerName = Environment.GetEnvironmentVariable("BlobContainerName");

                log.LogInformation($"Connection String: {connectionString}");
                log.LogInformation($"Container Name: {containerName}");

                // BlobServiceClient���g�p���ăX�g���[�W�A�J�E���g�ɐڑ�
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // �t���p�X��Blob���w��
                string fullPath = $"images/{blobName}";
                log.LogInformation($"Attempting to access blob: {fullPath} in container: {containerName}");

                // BlobClient���擾����Blob���_�E�����[�h
                BlobClient blobClient = containerClient.GetBlobClient(fullPath);
                var blobDownloadInfo = await blobClient.DownloadAsync();

                // �_�E�����[�h����Blob��MemoryStream�ɃR�s�[
                var memoryStream = new MemoryStream();
                await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // �摜��FileStreamResult�Ƃ��ĕԂ�
                return new FileStreamResult(memoryStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                // �G���[�����������ꍇ�̃��O�o��
                log.LogError($"Error retrieving blob: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
