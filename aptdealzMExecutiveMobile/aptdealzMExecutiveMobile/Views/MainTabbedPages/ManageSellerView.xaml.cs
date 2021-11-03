using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.Popup;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageSellerView : ContentView
    {
        #region [ Objects ]      
        private List<Seller> mSellers;
        private string filterBy = FilterEnums.ID.ToString();
        private string searchValue = string.Empty;
        private bool isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Constructor ]
        public ManageSellerView()
        {
            try
            {
                InitializeComponent();
                mSellers = new List<Seller>();
                pageNo = 1;
                GetAllSellers(filterBy, searchValue, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private async void GetAllSellers(string FilterBy = "", string SearchValue = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                SellerManagementAPI sellerManagementAPI = new SellerManagementAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await sellerManagementAPI.GetAllSellers(FilterBy, SearchValue, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var sellers = result.ToObject<List<Seller>>();
                    if (pageNo == 1)
                    {
                        mSellers.Clear();
                    }

                    foreach (var mSeller in sellers)
                    {
                        if (mSellers.Where(x => x.UserId == mSeller.UserId).Count() == 0)
                            mSellers.Add(mSeller);
                    }
                    BindList(mSellers);
                }
                else
                {
                    lstManageSeller.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindList(List<Seller> mSeller)
        {
            try
            {
                if (mSeller != null && mSeller.Count > 0)
                {
                    lstManageSeller.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstManageSeller.ItemsSource = mSeller.ToList();
                }
                else
                {
                    lstManageSeller.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/BindList: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        #region [ Header Navigation ]
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage(Constraints.Str_Home));
        }
        #endregion

        #region [ Filtering ]
        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            var Tab = (Frame)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    var sortby = new FilterPopup(filterBy);
                    sortby.isRefresh += (s1, e1) =>
                    {
                        string result = s1.ToString();
                        if (!Common.EmptyFiels(result))
                        {
                            filterBy = result;
                            if (filterBy == FilterEnums.ID.ToString())
                            {
                                lblFilterBy.Text = filterBy;
                            }
                            else
                            {
                                lblFilterBy.Text = filterBy.ToCamelCase();
                            }
                            pageNo = 1;
                            mSellers.Clear();
                            GetAllSellers(filterBy, searchValue, isAssending);
                        }
                    };
                    await PopupNavigation.Instance.PushAsync(sortby);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ManageSellerView/CustomEntry_Unfocused: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ImgASC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_ASC : Constraints.Sort_ASC_Dark;
                var ImgDSC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_DSC : Constraints.Sort_DSC_Dark;

                if (ImgSort.Source.ToString().Replace("File: ", "") == ImgASC)
                {
                    ImgSort.Source = ImgDSC;
                    isAssending = false;
                }
                else
                {
                    ImgSort.Source = ImgASC;
                    isAssending = true;
                }
                pageNo = 1;
                mSellers.Clear();
                GetAllSellers(filterBy, searchValue, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetAllSellers(filterBy, entrSearch.Text, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mSellers.Clear();
                    GetAllSellers(filterBy, searchValue, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mSellers);
        }
        #endregion

        #region [ Listing ]    
        private async void GrdSeller_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            if (GridExp.IsEnabled)
            {
                try
                {
                    GridExp.IsEnabled = false;
                    var mSeller = GridExp.BindingContext as Seller;
                    await Navigation.PushAsync(new MainTabbedPage("ManageSeller", sellerId: mSeller.UserId));
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ManageSellerView/GrdSeller_Tapped: " + ex.Message);
                }
                finally
                {
                    GridExp.IsEnabled = true;
                }
            }
        }

        private void lstManageSeller_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstManageSeller.SelectedItem = null;
        }

        private void lstManageSeller_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstManageSeller.IsRefreshing = true;
                pageNo = 1;
                mSellers.Clear();
                GetAllSellers(filterBy, searchValue, isAssending);
                lstManageSeller.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/lstManageSeller_Refreshing: " + ex.Message);
            }
        }

        private void lstManageSeller_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mSellers.Count < 10)
                    return;
                if (this.mSellers.Count == 0)
                    return;

                var lastrequirement = this.mSellers[this.mSellers.Count - 1];
                var lastAppearing = (Seller)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mSellers.Count() >= totalAspectedRow)
                        {
                            GetAllSellers(filterBy, searchValue, isAssending, false);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BtnManageSellers_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (ImageButton)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }

                var mSeller = imgExp.BindingContext as Seller;
                if (mSeller != null && mSeller.ArrowImage == Constraints.Arrow_Right)
                {
                    mSeller.ArrowImage = Constraints.Arrow_Down;

                    mSeller.GridBg = (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor8"] : Color.Transparent;
                    mSeller.MoreDetail = true;
                    mSeller.HideDetail = false;
                    mSeller.NameFont = 15;
                }
                else
                {
                    mSeller.ArrowImage = Constraints.Arrow_Right;
                    mSeller.GridBg = Color.Transparent;
                    mSeller.MoreDetail = false;
                    mSeller.HideDetail = true;
                    mSeller.NameFont = 13;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ManageSellerView/BtnManageSellers_Tapped: " + ex.Message);
            }
        }
        #endregion

        #endregion

    }
}