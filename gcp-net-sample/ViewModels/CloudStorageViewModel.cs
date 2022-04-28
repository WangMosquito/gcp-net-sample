namespace gcp_net_sample.ViewModels
{
    public class CloudStorageViewModel
    {
        public bool MissingBucketName { get; set; } = false;
        public string Content { get; set; } = "";
        public bool SavedNewContent { get; set; } = false;
        public string MediaLink { get; set; } = "";
    }
}
