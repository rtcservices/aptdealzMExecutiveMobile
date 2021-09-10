using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class Category
    {
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
