using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class State
    {
        [JsonProperty("stateId")]
        public int stateId { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
