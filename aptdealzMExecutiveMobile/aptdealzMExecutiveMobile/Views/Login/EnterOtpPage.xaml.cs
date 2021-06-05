using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterOtpPage : ContentPage
    {
        #region Objects       
        #endregion

        #region Constructor
        public EnterOtpPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods     
        #endregion

        #region Events      
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(ImgBack);
            Navigation.PopAsync();
        }

        private void TxtOtpOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtOtpOne.Text) && txtOtpOne.Text.Length == 0)
            {
                imgOtpOne.IsVisible = false;
            }
            else if (!string.IsNullOrEmpty(txtOtpOne.Text) && txtOtpOne.Text.Length == 1)
            {
                imgOtpOne.IsVisible = true;
                imgOtpTwo.IsVisible = false;
            }
            else if (!string.IsNullOrEmpty(txtOtpOne.Text) && txtOtpOne.Text.Length == 2)
            {
                imgOtpTwo.IsVisible = true;
                imgOtpThree.IsVisible = false;
            }
            else if (!string.IsNullOrEmpty(txtOtpOne.Text) && txtOtpOne.Text.Length == 3)
            {
                imgOtpThree.IsVisible = true;
                imgOtpFour.IsVisible = false;
                BtnSubmit.BackgroundColor = Color.FromHex("#C1C1C1");
            }
            else if (!string.IsNullOrEmpty(txtOtpOne.Text) && txtOtpOne.Text.Length == 4)
            {
                imgOtpFour.IsVisible = true;
                BtnSubmit.BackgroundColor = Color.FromHex("#006027");
            }
        }

        private void FrmShadow_Tapped(object sender, EventArgs e)
        {
            txtOtpOne.Focus();
        }
        #endregion

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}