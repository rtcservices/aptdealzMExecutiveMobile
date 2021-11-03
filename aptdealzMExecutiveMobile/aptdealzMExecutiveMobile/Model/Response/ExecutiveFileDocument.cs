using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class ExecutiveFileDocument
    {
        [JsonProperty("fileUri")]
        public string FileUri { get; set; }

        [JsonProperty("relativePath")]
        public string RelativePath { get; set; }
    }
}
