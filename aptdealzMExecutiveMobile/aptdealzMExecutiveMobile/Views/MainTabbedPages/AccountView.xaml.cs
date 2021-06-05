using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountView : ContentView
    {
        #region Objects      
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public AccountView()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods       
        #endregion

        #region Events        
        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(ImgMenu);
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgNotification);
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgQuestion);
        }


        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, StkBack);
            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void ImgCamera_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgCamera);
        }

        private void ImgEdit_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgEdit);
        }

        private void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(null, BtnUpdate);
        }

        private void frmState_Tapped(object sender, EventArgs e)
        {
            pckState.Focus();
        }

        private void pckState_Unfocused(object sender, FocusEventArgs e)
        {
            Xamarin.Forms.Picker picker = (Xamarin.Forms.Picker)sender;
            if (picker.SelectedItem != null)
            {
                lblState.Text = picker.SelectedItem.ToString();
                lblState.TextColor = Color.FromHex("#1A1818");
            }
        }
        #endregion
    }
}