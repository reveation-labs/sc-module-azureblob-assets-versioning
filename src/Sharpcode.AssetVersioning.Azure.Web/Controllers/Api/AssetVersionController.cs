using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Module.Core.Models.AzureBlob;
using VirtoCommerce.Module.Core.Services;

namespace VirtoCommerce.Module.Web.Controllers.Api
{
    [Route("api/assetversion")]
    public class AssetVersionController : Controller
    {
        private readonly ILogger<AssetVersionController> _logger;
        private readonly IAssetVersionService _assetVersionService;

        public AssetVersionController(ILogger<AssetVersionController> logger, IAssetVersionService assetVersionService)
        {
            _logger = logger;
            _assetVersionService = assetVersionService;
        }

        [HttpGet]
        [Route("containerblobs")]
        [Authorize]
        public async Task<ActionResult<List<string>>> GetContainerBlobs(string containerName)
        {
            try
            {
                var result = await _assetVersionService.GetContainerBlobs(containerName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetAllBlob api request: {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updateblobmetadata")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateBlobMetadata(string containerName, string blobName)
        {
            try
            {
                var metadata = new Dictionary<string, string>()
                {
                    {"Blob1", "BlobTest" },
                    {"Blob2", "BlobValue" }
                };
                var blobClient = await _assetVersionService.UpdateBlobMetadata(containerName, blobName, metadata);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in update blob metadata request: {ex}");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("blobproperties")]
        [Authorize]
        public async Task<ActionResult<string>> GetBlobProperties(string containerName, string blobName, string versionId)
        {
            try
            {
                var blobClient = await _assetVersionService.GetBlobProperties(containerName, blobName, versionId);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in blob properties request: {ex}");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("blobversions")]
        [Authorize]
        public ActionResult<List<AzureBlobItem>> GetBlobVersions(string containerName, string blobName)
        {
            try
            {
                var blobClient = _assetVersionService.GetBlobVersions(containerName, blobName);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in download blob request: {ex}");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("downloadblob")]
        [Authorize]
        public async Task<ActionResult<bool>> DownloadBlob(string containerName, string blobName, string versionId)
        {
            try
            {
                var blobClient = await _assetVersionService.DownloadBlob(containerName, blobName, versionId);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in blob properties request: {ex}");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("makecurrentversion")]
        [Authorize]
        public async Task<ActionResult<bool>> MakeCurrentVersion(string containerName, string blobName, string versionId)
        {
            try
            {
                var blobClient = await _assetVersionService.MakeCurrentVersion(containerName, blobName, versionId);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in blob make current version request: {ex}");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("uploadblob")]
        [Authorize]
        public async Task<ActionResult<string>> UploadBlob(string containerName, IFormFile file)
        {
            try
            {
                var blobClient = await _assetVersionService.UploadBlob(containerName, file.FileName, file);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in upload blob api request: {ex}");
                return BadRequest(ex);
            }
        }
        [HttpDelete]
        [Route("deleteblob")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteBlob(string containerName, string blobName)
        {
            try
            {
                var blobClient = await _assetVersionService.DeleteBlob(containerName, blobName);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in delete blob api request: {ex}");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("restoreblob")]
        [Authorize]
        public async Task<ActionResult<string>> RestoreBlob(string containerName, string blobName)
        {
            try
            {
                var blobClient = await _assetVersionService.RestoreBlob(containerName, blobName);
                return Ok(blobClient);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in restore blob api request: {ex}");
                return BadRequest(ex);
            }
        }
    }
}
