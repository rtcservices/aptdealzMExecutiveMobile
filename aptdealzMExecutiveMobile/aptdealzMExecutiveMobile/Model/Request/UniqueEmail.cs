using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Request
{
    public class UniqueEmail
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
