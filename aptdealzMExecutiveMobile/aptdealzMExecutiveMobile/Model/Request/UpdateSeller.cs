using aptdealzMExecutiveMobile.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace aptdealzMExecutiveMobile.Model.Request
{
    public class UpdateSeller
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("alternativePhoneNumber")]
        public string AlternativePhoneNumber { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        [JsonProperty("billingAddresses")]
        public List<BillingAddress> BillingAddresses { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subCategories")]
        public List<string> SubCategories { get; set; }

        [JsonProperty("experience")]
        public string Experience { get; set; }

        [JsonProperty("areaOfSupply")]
        public string AreaOfSupply { get; set; }

        [JsonProperty("gstin")]
        public string Gstin { get; set; }

        [JsonProperty("pan")]
        public string Pan { get; set; }

        [JsonProperty("bankAccountNumber")]
        public string BankAccountNumber { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("ifsc")]
        public string Ifsc { get; set; }

        [JsonProperty("documents")]
        public List<string> Documents { get; set; }

        [JsonProperty("commissionRate")]
        public int CommissionRate { get; set; }
    }
}
