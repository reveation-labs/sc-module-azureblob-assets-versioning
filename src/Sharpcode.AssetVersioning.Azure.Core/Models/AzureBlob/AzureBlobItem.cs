
using System;
using System.Collections.Generic;
namespace VirtoCommerce.Module.Core.Models.AzureBlob
{
    public class AzureBlobItem
    {
        public string BlobName { get; set; }
        public string Size { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string URL { get; set; }
        public bool? IsLatestVersion { get; set; }
        public string Snapshot { get; set; }
        public string VersionId { get; set; }
        public bool Deleted { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
