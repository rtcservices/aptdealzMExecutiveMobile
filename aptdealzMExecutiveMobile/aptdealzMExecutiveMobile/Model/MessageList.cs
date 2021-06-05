using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace aptdealzMExecutiveMobile.Model
{
    public class MessageList
    {
        public string Message { get; set; }
        public LayoutOptions MessagePosition { get; set; }
        public Thickness MessageMargin { get; set; }
        public Color MessageBackgroundColor { get; set; } = Color.FromHex("#F5F4F3");
        public Color MessageTextColor { get; set; } = Color.FromHex("#515151");
        public string UserName { get; set; }
        public string Time { get; set; }
        public string ContactImage { get; set; } = "imgContact.jpg";
        public string BuyerImage { get; set; } = "imgBuyer.png";
        public bool IsContact { get; set; } = false;
        public bool IsBuyer { get; set; } = false;
    }
}
