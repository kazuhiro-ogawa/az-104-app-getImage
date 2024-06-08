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
                // クエリパラメータからblobNameを取得
                string blobName = req.Query["blobName"];
                if (string.IsNullOrEmpty(blobName))
                {
                    return new BadRequestObjectResult("Please provide a valid blobName.");
                }

                // 環境変数からストレージ接続文字列とコンテナ名を取得
                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                string containerName = Environment.GetEnvironmentVariable("BlobContainerName");

                log.LogInformation($"Connection String: {connectionString}");
                log.LogInformation($"Container Name: {containerName}");

                // BlobServiceClientを使用してストレージアカウントに接続
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // フルパスでBlobを指定
                string fullPath = $"images/{blobName}";
                log.LogInformation($"Attempting to access blob: {fullPath} in container: {containerName}");

                // BlobClientを取得してBlobをダウンロード
                BlobClient blobClient = containerClient.GetBlobClient(fullPath);
                var blobDownloadInfo = await blobClient.DownloadAsync();

                // ダウンロードしたBlobをMemoryStreamにコピー
                var memoryStream = new MemoryStream();
                await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // 画像をFileStreamResultとして返す
                return new FileStreamResult(memoryStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                // エラーが発生した場合のログ出力
                log.LogError($"Error retrieving blob: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
