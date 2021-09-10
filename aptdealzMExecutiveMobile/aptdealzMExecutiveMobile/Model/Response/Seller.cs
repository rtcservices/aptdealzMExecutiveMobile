using aptdealzMExecutiveMobile.Utility;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Drawing;

namespace aptdealzMExecutiveMobile.Model.Response
{
    public class Seller : INotifyPropertyChanged
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("sellerNo")]
        public string SellerNo { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        #region [ Extra Properties ]
        [JsonIgnore]
        private string _ArrowImage { get; set; } = Constraints.Arrow_Right;

        [JsonIgnore]
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        [JsonIgnore]
        private double _NameFont { get; set; } = 13;
        [JsonIgnore]
        public double NameFont
        {
            get { return _NameFont; }
            set { _NameFont = value; PropertyChangedEventArgs("NameFont"); }
        }

        [JsonIgnore]
        private bool _MoreDetail { get; set; } = false;
        [JsonIgnore]
        public bool MoreDetail
        {
            get { return _MoreDetail; }
            set { _MoreDetail = value; PropertyChangedEventArgs("MoreDetail"); }
        }

        [JsonIgnore]
        private bool _HideDetail { get; set; } = true;
        [JsonIgnore]
        public bool HideDetail
        {
            get { return _HideDetail; }
            set { _HideDetail = value; PropertyChangedEventArgs("HideDetail"); }
        }

        [JsonIgnore]
        private Color _GridBg { get; set; } = Color.Transparent;
        [JsonIgnore]
        public Color GridBg
        {
            get { return _GridBg; }
            set { _GridBg = value; PropertyChangedEventArgs("GridBg"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
