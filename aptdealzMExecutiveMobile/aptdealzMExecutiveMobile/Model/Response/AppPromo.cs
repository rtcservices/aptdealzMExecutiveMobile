using Newtonsoft.Json;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class AppPromo
    {
        [JsonProperty("promoId")]
        public string PromoId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
