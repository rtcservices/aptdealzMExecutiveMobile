using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region Objects
        // create objects here
        #endregion

        #region Constructor
        public LoginPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        #endregion

        #region Events
        // create events here
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(ImgBack);
            Navigation.PopAsync();
        }

        private void StkRemember_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(null, null, null, StkRemember);
                var selectedImage = imgCheck.Source.ToString().Replace("File: ", string.Empty);
                if (selectedImage == "iconCheck.png")
                    imgCheck.Source = "iconUncheck.png";
                else
                    imgCheck.Source = "iconCheck.png";
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/StkRemember_Tapped" + ex.Message);
            }
        }

        private void StkSignup_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnGetOTP_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(null, BtnGetOTP);
            Navigation.PushAsync(new EnterOtpPage());
        }
        #endregion
    }
}