using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spalshscreen : ContentPage
    {
        #region Constructor
        public Spalshscreen()
        {
            InitializeComponent();
        }
        #endregion

        #region Method
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5 * 1000);
            App.Current.MainPage = new NavigationPage(new WelcomePage());
        }
        #endregion
    }
}