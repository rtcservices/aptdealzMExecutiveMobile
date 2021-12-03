using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Request
{
    public class ExecutiveDetails
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("profilePhoto")]
        public string ProfilePhoto { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("isNotificationMute")]
        public bool IsNotificationMute { get; set; }

    }
}
