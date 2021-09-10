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

            MessagingCenter.Unsubscribe<string>(this, "NotificationCount");
            MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
                        || selectedView == Constraints.Str_Account || selectedView == "About"
                        || selectedView == "Support")
                    {
                        isNavigate = true;
                        selectedView = Constraints.Str_Home;
                        BindViews(Constraints.Str_Home);
                    }
                    else if (selectedView == Constraints.Str_ManageSeller)
                    {
                        isNavigate = true;
                        Navigation.PopAsync();
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
                else if (view == Constraints.Str_Support)
                {
                    GrdTab.IsVisible = false;
                    grdMain.Children.Add(new ContactSupportView());
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

            imgHome.Source = Constraints.Img_Home;
            imgAddSeller.Source = Constraints.Img_AddSeller;
            imgManage.Source = Constraints.Img_ManageSeller;
            imgAccount.Source = Constraints.Img_Account;

            lblHome.TextColor = (Color)App.Current.Resources["appColor4"];
            lblAddSeller.TextColor = (Color)App.Current.Resources["appColor4"];
            lblManage.TextColor = (Color)App.Current.Resources["appColor4"];
            lblAccount.TextColor = (Color)App.Current.Resources["appColor4"];
        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

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
                    Common.DisplayErrorMessage("MainTabbedPage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void Tab_Tapped(object sender, EventArgs e)
        {
            var stack = (StackLayout)sender;
            if (stack.IsEnabled)
            {
                try
                {
                    stack.IsEnabled = false;
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
                finally
                {
                    stack.IsEnabled = true;
                }
            }
        }
        #endregion
    }
}