using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Module.Core.Models.AzureBlob;
using VirtoCommerce.Module.Core.Options;
using VirtoCommerce.Module.Core.Services;

namespace VirtoCommerce.Module.Data.Services
{
    public class AssetVersionService : IAssetVersionService
    {
        private readonly ILogger<IAssetVersionService> _logger;
        private readonly AzureBlobOptions _azureBlobOption;
        public AssetVersionService(ILogger<IAssetVersionService> logger, IOptions<AzureBlobOptions> azureBlobOptions)
        {
            _logger = logger;
            _azureBlobOption = azureBlobOptions.Value;
        }

        private BlobContainerClient GetBlobContainerClient(string blobContainerName)
        {
            return new BlobContainerClient(_azureBlobOption.ConnectionString, blobContainerName);
        }

        public async Task<List<AzureBlobItem>> GetContainerBlobs(string containerName)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var pageSegment = blobContainerClient.GetBlobsAsync().AsPages(default, 10);
                var blobList = new List<AzureBlobItem>();
                await foreach (var page in pageSegment)
                {
                    foreach (var blob in page.Values)
                    {
                        blobList.Add(new AzureBlobItem
                        {
                            BlobName = blob.Name,
                            CreatedOn = blob.Properties.CreatedOn,
                            LastModified = blob.Properties.LastModified,
                            IsLatestVersion = blob.IsLatestVersion,
                            Snapshot = blob.Snapshot,
                            VersionId = blob.VersionId,
                            Deleted = blob.Deleted,
                            Metadata = blob.Metadata
                        });
                    }
                }
                return blobList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while making a request to get all container blob: {ex}");
                return new List<AzureBlobItem>();
            }
        }

        public async Task<string> UpdateBlobMetadata(string containerName, string blobName, Dictionary<string, string> metadata)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
                var metadataResponse = await blockBlobClient.SetMetadataAsync(metadata);
                return metadataResponse.Value.VersionId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating a blob: {ex}");
                return null;
            }
        }

        public async Task<IDictionary<string, string>> GetBlobProperties(string containerName, string blobName, string blobVersion)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName).WithVersion(blobVersion);
                var blobProperties = await blockBlobClient.GetPropertiesAsync();
                if (blobProperties == null || blobProperties.Value.Metadata.Count <= 0)
                {
                    return null;
                }
                return blobProperties.Value.Metadata;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching blob properties: {ex}");
                return null;
            }
        }

        public List<AzureBlobItem> GetBlobVersions(string containerName, string blobName)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blobVersions = blobContainerClient.GetBlobs(BlobTraits.Metadata, BlobStates.Version, blobName)
                                        .OrderByDescending(o => o.VersionId).Where(x => x.Name == blobName);
                if (!blobVersions.Any())
                {
                    return null;
                }

                var blobItems = new List<AzureBlobItem>();
                foreach (var blob in blobVersions)
                {
                    blobItems.Add(new AzureBlobItem
                    {
                        BlobName = blob.Name,
                        CreatedOn = blob.Properties.CreatedOn,
                        LastModified = blob.Properties.LastModified,
                        IsLatestVersion = blob.IsLatestVersion,
                        Snapshot = blob.Snapshot,
                        VersionId = blob.VersionId,
                        Deleted = blob.Deleted,
                        Metadata = blob.Metadata
                    });
                }
                return blobItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching blob versions: {ex}");
                return null;
            }
        }

        public async Task<bool> DownloadBlob(string containerName, string blobName, string versionId)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blobUriBuilder = new BlobUriBuilder(blobContainerClient.Uri)
                {
                    BlobName = blobName,
                    VersionId = versionId
                };
                var blobClient = new BlobClient(blobUriBuilder.ToUri());
                var fileName = blobName.Split('.');
                await blobClient.DownloadToAsync($"{_azureBlobOption.BlobDownloadPath}\\{fileName[0]}_{versionId.Replace(':', '.')}.{fileName[1]}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while downloading a blob: {ex}");
                return false;
            }
        }

        public async Task<bool> MakeCurrentVersion(string containerName, string blobName, string versionId)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blobClient = blobContainerClient.GetBlockBlobClient(blobName);
                var versionClient = blobClient.WithVersion(versionId);
                await blobClient.SyncCopyFromUriAsync(versionClient.Uri);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while making a blob as current version: {ex}");
                return false;
            }
        }

        public async Task<string> UploadBlob(string containerName, string blobName, IFormFile file)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var uploadResponse = await blockBlobClient.UploadAsync(ms);
                var versionId = uploadResponse.Value.VersionId;
                return versionId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while uploading a blob: {ex}");
                return null;
            }
        }

        public async Task<bool> DeleteBlob(string containerName, string blobName)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
                await blockBlobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting a blob: {ex}");
                return false;
            }
        }

        public async Task<bool> RestoreBlob(string containerName, string blobName)
        {
            try
            {
                var blobContainerClient = GetBlobContainerClient(containerName);
                var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
                await blockBlobClient.UndeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while restoring a blob: {ex}");
                return false;
            }
        }
    }
}
