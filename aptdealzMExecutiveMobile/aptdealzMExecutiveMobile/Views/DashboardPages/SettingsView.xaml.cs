using System;
using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void BtnLight_Clicked(object sender, EventArgs e)
        {
            Application.Current.UserAppTheme = OSAppTheme.Light;
        }

        private void BtnDark_Clicked(object sender, EventArgs e)
        {
            Application.Current.UserAppTheme = OSAppTheme.Dark;
        }

        private void pkAlertTone_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnAlertTone_Clicked(object sender, EventArgs e)
        {
            pkAlertTone.Focus();
        }

        private void BtnMuteNotifications_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (BtnMuteNotifications.Source.ToString().Replace("File: ", "") == Constraints.Img_SwitchOff)
                {
                    BtnMuteNotifications.Source = Constraints.Img_SwitchOn;
                }
                else
                {
                    BtnMuteNotifications.Source = Constraints.Img_SwitchOff;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/BtnMuteNotifications_Clicked: " + ex.Message);
            }
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }
    }
}