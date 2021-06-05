using aptdealzMExecutiveMobile.Model;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageView : ContentView
    {
        #region Objects
        List<ManageSellerList> mManageSellerList = new List<ManageSellerList>();

        public event EventHandler isRefresh;
        #endregion

        #region Construter
        public ManageView()
        {
            InitializeComponent();
            BindListItems();
        }
        #endregion

        #region Methods
        void BindListItems()
        {
            try
            {
                mManageSellerList.Add(new ManageSellerList()
                {
                    SellerId = "SEL#101",
                    Active = "Active",
                    SellerName = "Alex Smith",
                    SellerAddress = "LA, US",
                    Date = "12-01-2021"
                });
                mManageSellerList.Add(new ManageSellerList()
                {
                    SellerId = "SEL#102",
                    Active = "Active",
                    SellerName = "Max",
                    SellerAddress = "LA, US",
                    Date = "25-01-2021"
                });
                mManageSellerList.Add(new ManageSellerList()
                {
                    SellerId = "SEL#103",
                    Active = "Disabled",
                    SellerName = "Stafin",
                    SellerAddress = "LA, US",
                    Date = "04-02-2021"
                });

                lstManageSeller.ItemsSource = mManageSellerList.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageView/BindListItems: " + ex.Message);
            }
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

        private void BtnManageSellers_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectGrid = (ImageButton)sender;
                var setHight = (ViewCell)selectGrid.Parent.Parent.Parent.Parent;
                if (setHight != null)
                {
                    setHight.ForceUpdateSize();
                }

                var response = (ManageSellerList)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mManageSellerList)
                    {
                        if (selectedImage.ArrowImage == "iconRightArrow.png")
                        {
                            selectedImage.ArrowImage = "iconRightArrow.png";
                            selectedImage.GridBg = Color.Transparent;
                            selectedImage.NameFont = 13;
                            selectedImage.MoreDetail = false;
                        }
                        else
                        {
                            selectedImage.ArrowImage = "iconDownArrow.png";
                            selectedImage.GridBg = Color.FromHex("#F0F0F0");
                            selectedImage.NameFont = 15;
                            selectedImage.MoreDetail = true;
                        }
                    }
                    if (response.ArrowImage == "iconRightArrow.png")
                    {
                        response.ArrowImage = "iconDownArrow.png";
                        response.GridBg = Color.FromHex("#F0F0F0");
                        response.NameFont = 15;
                        response.MoreDetail = true;
                    }
                    else
                    {
                        response.ArrowImage = "iconRightArrow.png";
                        response.GridBg = Color.Transparent;
                        response.NameFont = 13;
                        response.MoreDetail = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageView/BtnManageSellers_Tapped: " + ex.Message);
            }
        }

        private void lstManageSeller_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstManageSeller.SelectedItem = null;
            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("ManageSeller"));
        }

        private async void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup();
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageView/FrmSortBy_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}