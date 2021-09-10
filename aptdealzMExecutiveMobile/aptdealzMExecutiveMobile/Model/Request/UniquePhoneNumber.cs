using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Request
{
    public class UniquePhoneNumber
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
