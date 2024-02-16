namespace VirtoCommerce.Module.Core.Options
{
    public class AzureBlobOptions
    {
        public const string SectionName = "Assets:AzureBlobStorage";
        public string ConnectionString { get; set; }
        public string BlobDownloadPath { get; set; }
    }
}
