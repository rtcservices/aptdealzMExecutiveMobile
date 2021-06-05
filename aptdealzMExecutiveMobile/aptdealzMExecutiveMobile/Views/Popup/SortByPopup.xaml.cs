using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortByPopup : PopupPage
    {
        public event EventHandler isRefresh;

        public SortByPopup()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void BindSource(string viewSource)
        {
            if (!string.IsNullOrEmpty(viewSource))
            {
                if (viewSource == "ID")
                {
                    ClearSource();
                    BtnID.Source = "iconRadioSelect.png";
                }
                else if (viewSource == "Status")
                {
                    ClearSource();
                    BtnStatus.Source = "iconRadioSelect.png";
                }
                else if (viewSource == "Amount")
                {
                    ClearSource();
                    BtnAmount.Source = "iconRadioSelect.png";
                }
                else
                {
                    ClearSource();
                    BtnID.Source = "iconRadioSelect.png";
                }
            }
        }

        void ClearSource()
        {
            BtnID.Source = "iconRadioUnselect.png";
            BtnStatus.Source = "iconRadioUnselect.png";
            BtnAmount.Source = "iconRadioUnselect.png";
        }

        private void BtnID_Clicked(object sender, EventArgs e)
        {
            BindSource("ID");
            isRefresh?.Invoke("ID", null);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnStatus_Clicked(object sender, EventArgs e)
        {
            BindSource("Status");
            isRefresh?.Invoke("Status", null);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnAmount_Clicked(object sender, EventArgs e)
        {
            BindSource("Amount");
            isRefresh?.Invoke("Amount", null);
            PopupNavigation.Instance.PopAsync();
        }
    }
}