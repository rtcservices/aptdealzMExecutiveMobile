using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;
using aptdealzMExecutiveMobile.Utility;

namespace aptdealzMExecutiveMobile.Model.Response
{

    public class FAQResponse : INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }


        [JsonIgnore]
        private string _ArrowImage = Constraints.Img_GreenArrowDown;
        [JsonIgnore]
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }

        [JsonIgnore]
        private bool _ShowFaqDesc;
        [JsonIgnore]
        public bool ShowFaqDesc
        {
            get { return _ShowFaqDesc; }
            set { _ShowFaqDesc = value; PropertyChangedEventArgs("ShowFaqDesc"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
