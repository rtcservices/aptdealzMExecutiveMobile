using aptdealzMExecutiveMobile.Model;
using aptdealzMExecutiveMobile.Utility;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        #region Objecst
        // create objects here
        public List<CarousellImage> mCarousellImages = new List<CarousellImage>();
        #endregion

        #region Constructor
        public WelcomePage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindCarousallData();
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

        #region Events
        private void BtnLogin_Clicked(object sender, System.EventArgs e)
        {
            Common.BindAnimation(null, BtnLogin);
            Navigation.PushAsync(new Login.LoginPage());
        }
        #endregion

        private void FrmSkip_Tapped(object sender, System.EventArgs e)
        {
            //Common.BindAnimation(null, null, FrmSkip);
            Navigation.PushAsync(new Login.LoginPage());
        }
    }
}