using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class AuthenticatedUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}
