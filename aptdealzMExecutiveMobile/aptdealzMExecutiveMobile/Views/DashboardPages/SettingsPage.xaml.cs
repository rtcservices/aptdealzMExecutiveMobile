//using aptdealzMExecutiveMobile.Theme.Styles;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        #region [ Ctor ]
        public SettingsPage()
        {
            InitializeComponent();

            if (Common.mExecutiveDetails.IsNotificationMute)
            {
                BtnMuteNotifications.Source = Constraints.Img_SwitchOn;
                lblmute.Text = "On";
            }
            else
            {
                BtnMuteNotifications.Source = Constraints.Img_SwitchOff;
                lblmute.Text = "Off";
            }

        }
        #endregion

        #region [ Methods ]
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        } 
        #endregion

        #region [ Events ]
        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("SettingsPage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PushAsync(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private void BtnLight_Clicked(object sender, EventArgs e)
        {
            Settings.IsDarkMode = false;
            Application.Current.UserAppTheme = OSAppTheme.Light;
        }

        private void BtnDark_Clicked(object sender, EventArgs e)
        {
            Settings.IsDarkMode = true;
            Application.Current.UserAppTheme = OSAppTheme.Dark;
        }

        private void pkAlertTone_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnAlertTone_Clicked(object sender, EventArgs e)
        {
           // pkAlertTone.Focus();
        }

        private async void BtnMuteNotifications_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (BtnMuteNotifications.Source.ToString().Replace("File: ", "") == Constraints.Img_SwitchOff)
                {
                    BtnMuteNotifications.Source = Constraints.Img_SwitchOn;
                    Settings.IsMuteMode = true;
                    lblmute.Text = "On";
                }
                else
                {
                    BtnMuteNotifications.Source = Constraints.Img_SwitchOff;
                    Settings.IsMuteMode = false;
                    lblmute.Text = "Off";
                }

                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.UpdateUserMuteNotification(Settings.UserId, Settings.IsMuteMode);
                if (mResponse != null)
                {
                    if (!mResponse.Succeeded)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplaySuccessMessage(mResponse.Message);
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
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
        #endregion
    }
}