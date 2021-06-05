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
        #region Objects   
        public event EventHandler isRefresh;
        public List<HomeMenu> mHomeMenu = new List<HomeMenu>();
        #endregion

        #region Constructor
        public HomeView()
        {
            InitializeComponent();
            BindMenus();
        }
        #endregion

        #region Methods

        public void BindMenus()
        {
            mHomeMenu = new List<HomeMenu>()
            {
                new HomeMenu{Id=1, MenuImage="imgActiveRequirements.png", UiName="Add Seller"},
                new HomeMenu{Id=2, MenuImage="imgPostRequirements.png", UiName="Manage Sellers"},
                new HomeMenu{Id=3, MenuImage="imgNotifications.png", UiName="Notifications"},
                new HomeMenu{Id=4, MenuImage="imgProfile.png", UiName="Account"},
                new HomeMenu{Id=5, MenuImage="imgAboutAptDealz.png", UiName="About\nAptDealz"},
                new HomeMenu{Id=6, MenuImage="imgContactSupport.png", UiName="Contact\nSupport"},
            };

            flvMenus.FlowItemsSource = mHomeMenu.ToList();
        }
        #endregion

        #region Events   
        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(ImgMenu);
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgNotification);
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgQuestion);
        }

        private void BtnDivision_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stk = (Extention.CustomShadowFrame)sender;
                var menu = stk.BindingContext as HomeMenu;
                if (menu != null)
                {
                    if (menu.Id == 1)
                    {
                        Navigation.PushAsync(new MainTabbedPage("AddSeller"));
                    }
                    else if (menu.Id == 2)
                    {
                        Navigation.PushAsync(new MainTabbedPage("Manage"));
                    }
                    else if (menu.Id == 3)
                    {
                        Navigation.PushAsync(new NotificationPage());
                    }
                    else if (menu.Id == 4)
                    {
                        Navigation.PushAsync(new MainTabbedPage("Account"));
                    }
                    else if (menu.Id == 5)
                    {
                        Navigation.PushAsync(new MainTabbedPage("About"));
                    }
                    else if (menu.Id == 6)
                    {
                        Navigation.PushAsync(new ContactSupportPage());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/BtnDivision_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}