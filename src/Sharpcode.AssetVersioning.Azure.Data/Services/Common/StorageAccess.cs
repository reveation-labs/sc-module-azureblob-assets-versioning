using System;
using Azure.Storage.Blobs;

namespace VirtoCommerce.Module.Data.Services.Common
{
    public static class StorageAccess
    {
        public static BlobServiceClient CreateBlobServiceClient(string connectingString)
        {
            BlobServiceClient blobServiceClient;

            try
            {
                return blobServiceClient = new BlobServiceClient(connectingString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
