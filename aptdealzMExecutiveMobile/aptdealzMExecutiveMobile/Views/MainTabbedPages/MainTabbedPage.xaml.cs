using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.DashboardPages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region [ Objects ]       
        private string selectedView;
        private bool isNavigate = false;
        private string SellerId = string.Empty;
        #endregion

        #region [ Constructor ]    
        public MainTabbedPage(string OpenView, bool isNavigate = false, string sellerId = null)
        {
            InitializeComponent();
            this.isNavigate = isNavigate;
            SellerId = sellerId;
            selectedView = OpenView;
            BindViews(selectedView);
            GetProfile();

            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
            MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
            {
                if (!Common.EmptyFiels(Common.NotificationCount))
                {
                    lblNotificationCount.Text = count;
                    frmNotification.IsVisible = true;
                }
                else
                {
                    frmNotification.IsVisible = false;
                    lblNotificationCount.Text = string.Empty;
                }
            });
        }
        #endregion

        #region [ Methods ]
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (!Common.EmptyFiels(selectedView))
                {
                    if (selectedView == Constraints.Str_AddSeller || selectedView == Constraints.Str_Manage
                        || selectedView == Constraints.Str_Account || selectedView == Constraints.Str_About ||
                        selectedView == Constraints.Str_Settings || selectedView == Constraints.Str_ManageSeller ||
                        selectedView == Constraints.Str_Support)
                    {
                        isNavigate = true;

                        if (selectedView == Constraints.Str_Settings || selectedView == Constraints.Str_Support)
                            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));

                        selectedView = Constraints.Str_Home;
                        BindViews(Constraints.Str_Home);
                    }
                    //else if ()
                    //{
                    //    isNavigate = true;
                    //    Navigation.PopAsync();
                    //}

                    if (selectedView == Constraints.Str_Support)
                    {
                        if (App.chatStoppableTimer != null)
                        {
                            App.chatStoppableTimer.Stop();
                            App.chatStoppableTimer = null;
                        }
                    }
                }

                if (!isNavigate)
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

                isNavigate = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        async void GetProfile()
        {
            try
            {
                await DependencyService.Get<IProfileRepository>().GetMyProfileData();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/GetProfile: " + ex.Message);
            }
        }

        private Color BindTextColor()
        {
            return (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor4"] : (Color)App.Current.Resources["appColor6"];
        }

        private void BindViews(string view)
        {
            try
            {
                UnselectTab();
                if (view == Constraints.Str_Home)
                {
                    imgHome.Source = Constraints.Img_Home_Active;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new HomeView());
                }
                else if (view == Constraints.Str_AddSeller)
                {
                    imgAddSeller.Source = Constraints.Img_AddSeller_Active;
                    lblAddSeller.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new AddSellerView());
                }
                else if (view == Constraints.Str_ManageSeller)
                {
                    imgManage.Source = Constraints.Img_ManageSeller_Active;
                    lblManage.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new AddSellerView(SellerId));
                }
                else if (view == Constraints.Str_Manage)
                {
                    imgManage.Source = Constraints.Img_ManageSeller_Active;
                    lblManage.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new ManageSellerView());
                }
                else if (view == Constraints.Str_Account)
                {
                    imgAccount.Source = Constraints.Img_Account_Active;
                    lblAccount.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new AccountView());
                }
                else if (view == Constraints.Str_About)
                {
                    GrdTab.IsVisible = false;
                    grdMain.Children.Add(new AboutView());
                }
                else if (view == Constraints.Str_Support || view == Constraints.Str_FAQHelp)
                {
                    GrdTab.IsVisible = false;
                    grdMain.Children.Add(new ContactSupportView());
                }
                else if (view == Constraints.Str_Settings)
                {
                    GrdTab.IsVisible = false;
                    grdMain.Children.Add(new SettingsView());
                }
                else
                {
                    imgHome.Source = Constraints.Img_Home_Active;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new HomeView());
                }
                selectedView = view;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/BindViews: " + ex.Message);
            }
        }

        public void UnselectTab()
        {
            grdMain.Children.Clear();

            imgHome.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_Home : Constraints.Img_Home_Dark;
            imgAddSeller.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_AddSeller : Constraints.Img_AddSeller_Dark;
            imgManage.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_ManageSeller : Constraints.Img_ManageSeller_Dark;
            imgAccount.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_Account : Constraints.Img_Account_Dark;

            lblHome.TextColor = lblAddSeller.TextColor = lblManage.TextColor = lblAccount.TextColor = BindTextColor();
        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Settings));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            if (selectedView != Constraints.Str_Home)
            {
                Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
            }
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            if (ImgNotification.IsEnabled)
            {
                try
                {
                    ImgNotification.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("MainTabbedPage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    ImgNotification.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_FAQHelp));
        }

        private void Tab_Tapped(object sender, EventArgs e)
        {
            var stack = (StackLayout)sender;
            try
            {
                if (!Common.EmptyFiels(stack.ClassId))
                {
                    if (stack.ClassId == Constraints.Str_Home)
                    {
                        BindViews(Constraints.Str_Home);
                    }
                    else if (stack.ClassId == Constraints.Str_AddSeller)
                    {
                        this.isNavigate = true;
                        if (selectedView != Constraints.Str_AddSeller)
                        {
                            BindViews(Constraints.Str_AddSeller);
                        }
                    }
                    else if (stack.ClassId == Constraints.Str_Manage)
                    {
                        this.isNavigate = true;
                        if (selectedView != Constraints.Str_Manage)
                        {
                            BindViews(Constraints.Str_Manage);
                        }
                    }
                    else if (stack.ClassId == Constraints.Str_Account)
                    {
                        this.isNavigate = true;
                        if (selectedView != Constraints.Str_Account)
                        {
                            BindViews(Constraints.Str_Account);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/Tab_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}