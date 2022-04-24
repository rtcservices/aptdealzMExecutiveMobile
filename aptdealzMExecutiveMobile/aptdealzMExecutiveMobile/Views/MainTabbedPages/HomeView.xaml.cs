using aptdealzMExecutiveMobile.Model;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.DashboardPages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentView
    {
        #region [ Objects ]         
        public List<HomeMenu> mHomeMenu = new List<HomeMenu>();
        #endregion

        #region [ Constructor ]
        public HomeView()
        {
            try
            {
                InitializeComponent();
                BindMenus();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        public void BindMenus()
        {
            mHomeMenu = new List<HomeMenu>()
            {
                new HomeMenu{MenuImage="iconAddSeller.png", UiName="Add Seller",MenuName=Constraints.Str_AddSeller},
                new HomeMenu{MenuImage="iconManageSeller.png", UiName="Manage Sellers",MenuName=Constraints.Str_Manage},
                new HomeMenu{MenuImage="imgNotifications.png", UiName="Notifications",MenuName=Constraints.Str_Notifications},
                new HomeMenu{MenuImage="imgProfile.png", UiName="Account",MenuName=Constraints.Str_Account},
                new HomeMenu{MenuImage="imgAboutAptDealz.png", UiName="About\nAptDealz",MenuName=Constraints.Str_About},
                new HomeMenu{MenuImage="imgContactSupport.png", UiName="Contact\nSupport",MenuName=Constraints.Str_Support},
            };
            if (App.Current.Resources["BaseURL"].ToString().Contains("https://aptdealzstaging1api.azurewebsites.net"))
            {
                lblStag.IsVisible = true;
                lblStag.Text = "Stagging";
            }
            else if (App.Current.Resources["BaseURL"].ToString().Contains("https://aptdealzapidev.azurewebsites.net"))
            {
                lblStag.IsVisible = true;
                lblStag.Text = "Dev";
            }
            else
            {
                lblStag.IsVisible = false;
            }
            flvMenus.FlowItemsSource = mHomeMenu.ToList();
        }
        #endregion

        #region [ Events ]  
        //private void ImgMenu_Tapped(object sender, EventArgs e)
        //{
        //    //Common.OpenMenu();
        //}

        //private async void ImgNotification_Tapped(object sender, EventArgs e)
        //{
        //    var Tab = (Grid)sender;
        //    if (Tab.IsEnabled)
        //    {
        //        try
        //        {
        //            Tab.IsEnabled = false;
        //            await Navigation.PushAsync(new NotificationPage());
        //        }
        //        catch (Exception ex)
        //        {
        //            Common.DisplayErrorMessage("HomeView/ImgNotification_Tapped: " + ex.Message);
        //        }
        //        finally
        //        {
        //            Tab.IsEnabled = true;
        //        }
        //    }
        //}

        //private void ImgQuestion_Tapped(object sender, EventArgs e)
        //{

        //}

        private async void DashboardMenu_Tapped(object sender, EventArgs e)
        {
            var MenuTab = (Frame)sender;
            try
            {
                var mHomeMenu = MenuTab.BindingContext as HomeMenu;
                if (mHomeMenu != null && mHomeMenu.MenuName == Constraints.Str_Notifications)
                {
                    await Navigation.PushAsync(new NotificationPage());
                }
                //else if (mHomeMenu != null && mHomeMenu.MenuName == Constraints.Str_Support)
                //{
                //    await Navigation.PushAsync(new OtherPages.ContactSupportPage());
                //}
                else if (mHomeMenu != null && mHomeMenu.MenuName != null)
                {
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPage(mHomeMenu.MenuName, true));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/DashboardMenu_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}