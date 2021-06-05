using System.ComponentModel;
using Xamarin.Forms;

namespace aptdealzMExecutiveMobile.Model
{
    public class ManageSellerList : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public string Active { get; set; }
        public string SellerAddress { get; set; }
        public string Date { get; set; }


        private string _ArrowImage { get; set; } = "iconRightArrow.png";
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        private Color _GridBg { get; set; } = Color.Transparent;
        public Color GridBg
        {
            get { return _GridBg; }
            set { _GridBg = value; PropertyChangedEventArgs("GridBg"); }
        }

        private double _NameFont { get; set; } = 13;
        public double NameFont
        {
            get { return _NameFont; }
            set { _NameFont = value; PropertyChangedEventArgs("NameFont"); }
        }

        private bool _MoreDetail { get; set; } = false;
        public bool MoreDetail
        {
            get { return _MoreDetail; }
            set { _MoreDetail = value; PropertyChangedEventArgs("MoreDetail"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
