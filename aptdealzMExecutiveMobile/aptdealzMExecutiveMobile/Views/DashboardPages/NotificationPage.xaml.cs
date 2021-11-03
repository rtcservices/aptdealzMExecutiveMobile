﻿using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        #region [ Objects ]
        private List<NotificationData> mNotificationsList;
        #endregion

        #region [ Constructor ]
        public NotificationPage()
        {
            InitializeComponent();
            mNotificationsList = new List<NotificationData>();
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
            try
            {
                base.OnAppearing();
                GetNotification();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/Appearing: " + ex.Message);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private async Task GetNotification()
        {
            NotificationAPI notificationAPI = new NotificationAPI();
            if (!Common.EmptyFiels(Settings.UserToken))
            {
                if (Common.EmptyFiels(Common.Token))
                {
                    Common.Token = Settings.UserToken;
                }
            }
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await notificationAPI.GetAllNotificationsForUser();
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    if (result != null)
                    {
                        mNotificationsList = result.ToObject<List<NotificationData>>();
                        if (mNotificationsList != null && mNotificationsList.Count > 0)
                        {
                            lstNotification.IsVisible = true;
                            lblNoRecord.IsVisible = false;
                            lstNotification.ItemsSource = mNotificationsList.ToList();
                        }
                        else
                        {
                            lstNotification.IsVisible = false;
                            lblNoRecord.IsVisible = true;
                        }
                    }
                }
                else
                {
                    lblNoRecord.IsVisible = true;
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        lblNoRecord.Text = mResponse.Message;
                    else
                        lblNoRecord.Text = Constraints.Something_Wrong;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/GetNotification: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task SetNoficiationAsRead(string NotificationId)
        {
            try
            {
                var isReded = await DependencyService.Get<INotificationRepository>().SetUserNoficiationAsRead(NotificationId);
                if (isReded)
                {
                    mNotificationsList.Clear();
                    await GetNotification();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/GetNotification: " + ex.Message);
            }
        }

        private async Task SetUserNoficiationAsReadAndDelete(string NotificationId)
        {
            try
            {
                var isReded = await DependencyService.Get<INotificationRepository>().SetUserNoficiationAsReadAndDelete(NotificationId);
                if (isReded)
                {
                    mNotificationsList.Clear();
                    await GetNotification();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/SetUserNoficiationAsReadAndDelete: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgMenu);
        }

        private void ImgQuestion_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_FAQHelp));
        }

        private void ImgBack_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private void lstNotification_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstNotification.SelectedItem = null;
        }

        private async void lstNotification_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstNotification.IsRefreshing = true;
                mNotificationsList.Clear();
                await GetNotification();
                lstNotification.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/lstNotification_Refreshing: " + ex.Message);
            }
        }

        private async void ImgClose_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ImageButtonExp = (ImageButton)sender;
                if (ImageButtonExp != null)
                {
                    var notificationData = ImageButtonExp.BindingContext as NotificationData;
                    if (notificationData != null && !Common.EmptyFiels(notificationData.NotificationId))
                    {
                        await SetUserNoficiationAsReadAndDelete(notificationData.NotificationId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/ImgClose_Tapped: " + ex.Message);
            }
        }

        private async void GrdList_Tapped(object sender, EventArgs e)
        {
            try
            {
                var Tab = (Grid)sender;
                if (Tab.IsEnabled)
                {
                    try
                    {
                        Tab.IsEnabled = false;
                        var mNotification = Tab.BindingContext as NotificationData;
                        if (mNotification != null && !Common.EmptyFiels(mNotification.NotificationId))
                        {
                            await SetNoficiationAsRead(mNotification.NotificationId);
                            if (mNotification.NavigationScreen == (int)NavigationScreen.SupportChatDetails)
                            {
                                await Navigation.PushAsync(new MainTabbedPages.MainTabbedPage(Constraints.Str_Support));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.DisplayErrorMessage("NotificationPage/GrdList_Tapped: " + ex.Message);
                    }
                    finally
                    {
                        Tab.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/GrdList_Tapped: " + ex.Message);
            }
        }
        #endregion

        private void btnSettings_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Settings));
        }
    }
}