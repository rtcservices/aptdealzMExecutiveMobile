using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentView
    {
        Dictionary<int, string> mRingtones = new Dictionary<int, string>();

        #region [ Ctor ]
        public SettingsView()
        {
            try
            {
                InitializeComponent();

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    DependencyService.Get<IOpenWriteSettings>().GrantWriteSettings();
                }

                LoadRingtones();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/Ctor : " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private void LoadRingtones()
        {
            try
            {
                mRingtones = Xamarin.Forms.DependencyService.Get<IRingtoneManager>().GetRingtones();
                if (mRingtones != null && mRingtones.Count > 0)
                {
                    pkAlertTone.ItemsSource = mRingtones.Select(x => x.Value).OrderBy(x => x).ToList();
                    if (!Common.EmptyFiels(Settings.NotificationToneName))
                    {
                        pkAlertTone.SelectedItem = Settings.NotificationToneName;
                    }
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/LoadRingtones: " + ex.Message);
            }
        }
        #endregion

        #region [ Event ]
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
            try
            {
                if (!Common.EmptyFiels(pkAlertTone.SelectedItem as string))
                {
                    var tone = pkAlertTone.SelectedItem as string;
                    var selectedToneId = mRingtones.Where(x => x.Value == tone).FirstOrDefault().Key;

                    Xamarin.Forms.DependencyService.Get<IRingtoneManager>().PlayRingTone(selectedToneId);
                    Xamarin.Forms.DependencyService.Get<IRingtoneManager>().SaveRingTone(selectedToneId);

                    if (!Common.EmptyFiels(Settings.NotificationToneName))
                    {
                        pkAlertTone.SelectedItem = Settings.NotificationToneName;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/Picker_Unfocused: " + ex.Message);
            }
        }

        private void BtnAlertTone_Clicked(object sender, EventArgs e)
        {
            pkAlertTone.Focus();
        }
        #endregion
    }
}