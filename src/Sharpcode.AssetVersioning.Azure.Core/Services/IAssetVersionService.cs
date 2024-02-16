
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Module.Core.Models.AzureBlob;

namespace VirtoCommerce.Module.Core.Services
{
    public interface IAssetVersionService
    {
        Task<List<AzureBlobItem>> GetContainerBlobs(string containerName);
        Task<string> UpdateBlobMetadata(string containerName, string blobName, Dictionary<string, string> metadata);
        Task<IDictionary<string, string>> GetBlobProperties(string containerName, string blobName, string blobVersion);
        List<AzureBlobItem> GetBlobVersions(string containerName, string blobName);
        Task<string> UploadBlob(string containerName, string blobName, IFormFile file);
        Task<bool> DownloadBlob(string containerName, string blobName, string versionId);
        Task<bool> MakeCurrentVersion(string containerName, string blobName, string versionId);
        Task<bool> DeleteBlob(string containerName, string blobName);
        Task<bool> RestoreBlob(string containerName, string blobName);
    }
}
