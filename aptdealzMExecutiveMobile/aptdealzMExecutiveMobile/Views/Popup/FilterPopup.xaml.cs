using aptdealzMExecutiveMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup : PopupPage
    {
        #region [ Objects ]       
        public event EventHandler isRefresh;
        #endregion

        #region [ Constructor ]        
        public FilterPopup(string SortBy)
        {
            InitializeComponent();
            BindSource(SortBy);
        }
        #endregion

        #region [ Methods ]
        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindLabel();
        }

        private void BindLabel()
        {
            try
            {
                lblFirstType.Text = FilterEnums.ID.ToString();
                lblSecondType.Text = FilterEnums.Date.ToString();
                lblThirdType.Text = FilterEnums.Validity.ToString();
                lblFourType.Text = FilterEnums.Amount.ToString();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/BindLabel: " + ex.Message);
            }
        }

        private void BindSource(string viewSource)
        {
            try
            {
                if (!Common.EmptyFiels(viewSource))
                {
                    if (viewSource == FilterEnums.ID.ToString())
                    {
                        ClearSource();
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == FilterEnums.Date.ToString())
                    {
                        ClearSource();
                        imgSecondType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == FilterEnums.Validity.ToString())
                    {
                        ClearSource();
                        imgThirdType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == FilterEnums.Amount.ToString())
                    {
                        ClearSource();
                        imgFourType.Source = Constraints.Redio_Selected;
                    }
                    else
                    {
                        ClearSource();
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/BindSource: " + ex.Message);
            }
        }

        private void ClearSource()
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Redio_UnSelected;
            imgFourType.Source = Constraints.Redio_UnSelected;
        }
        #endregion

        #region [ Events ]
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource(FilterEnums.ID.ToString());
                isRefresh?.Invoke(FilterEnums.ID.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkFirstType: " + ex.Message);
            }
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource(FilterEnums.Date.ToString());
                isRefresh?.Invoke(FilterEnums.Date.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkSecondType: " + ex.Message);
            }
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource(FilterEnums.Validity.ToString());
                isRefresh?.Invoke(FilterEnums.Validity.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkThirdType: " + ex.Message);
            }
        }

        private void StkFourType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource(FilterEnums.Amount.ToString());
                isRefresh?.Invoke(FilterEnums.Amount.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkFourType: " + ex.Message);
            }
        }
        #endregion
    }
}