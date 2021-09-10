using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Model;
using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        #region [ Objects ]     
        public List<CarousellImage> mCarousellImages = new List<CarousellImage>();
        #endregion

        #region [ Constructor ]
        public WelcomePage()
        {
            InitializeComponent();
            Settings.IsViewWelcomeScreen = false;
        }
        #endregion

        #region [ Methods ]
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindCarousallData();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                        if (result)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("WelcomePage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        public void BindCarousallData()
        {
            mCarousellImages = new List<CarousellImage>()
            {
                new CarousellImage{ImageName="imgWelcomeOne.png"},
                new CarousellImage{ImageName="imgWelcomeTwo.png"},
                new CarousellImage{ImageName="imgWelcomeThree.png"},
            };
            Indicators.ItemsSource = cvWelcome.ItemsSource = mCarousellImages.ToList();
        }
        #endregion

        #region [ Events ]
        private async void BtnLogin_Clicked(object sender, System.EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnLogin);
                    await Navigation.PushAsync(new Login.LoginPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("WelcomePage/BtnLogin_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }
        #endregion

    }
}