using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Extention;
using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.DashboardPages;
using dotMorten.Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSellerView : ContentView, INotifyPropertyChanged
    {
        #region [ Properties ]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<string> _mCountriesData;
        public ObservableCollection<string> mCountriesData
        {
            get { return _mCountriesData; }
            set
            {
                _mCountriesData = value;
                OnPropertyChanged("mCountriesData");
            }
        }
        #endregion

        #region [ Object ]
        private SellerManagementAPI sellerManagementAPI;
        private SellerDetails mSellerDetail;

        private List<Category> mCategories;
        private List<SubCategory> mSubCategories;
        private List<string> selectedSubCategory;
        private List<string> documentList;

        private string SellerId = string.Empty;
        private string relativePath = string.Empty;
        private string relativeDocumentPath = string.Empty;
        private string ErrorMessage = string.Empty;

        private bool isChecked = false;
        private bool isFirstLoad = true;
        private bool isUpdatPhoto = false;
        private bool isDeleteSubCategory = true;
        #endregion

        #region [ Ctor ]
        public AddSellerView(string sellerId = null)
        {
            try
            {
                InitializeComponent();
                SellerId = sellerId;
                BindObjects();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]  

        #region [ Get / Bind Data ]
        private async void BindObjects()
        {
            try
            {
                sellerManagementAPI = new SellerManagementAPI();
                mCategories = new List<Category>();
                mSubCategories = new List<SubCategory>();
                selectedSubCategory = new List<string>();
                documentList = new List<string>();
                mSellerDetail = new SellerDetails();

                await GetCategory();
                await GetCountries();
                CapitalizeWord();

                if (!Common.EmptyFiels(SellerId))
                {
                    lblHeader.Text = "Update Seller";
                    // txtEmail.IsEnabled = false;
                    txtEmail.IsReadOnly = true;
                    BtnSubmit.IsEnabled = false;
                    StkPassword.IsVisible = false;
                    BtnSubmit.Text = "Update";
                    await BindSellerDetails();
                }
                else
                {
                    lblHeader.Text = "Add Seller";
                    //txtEmail.IsEnabled = true;
                    txtEmail.IsReadOnly = false;
                    BtnSubmit.IsEnabled = true;
                    StkPassword.IsVisible = true;
                    BtnSubmit.Text = "Submit";
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/BindObjects: " + ex.Message);
            }
        }

        private void CapitalizeWord()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                txtStreet.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtCity.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtState.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtLandmark.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                txtDescription.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtSupplyArea.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                txtGstNumber.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtPan.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtBankName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtIfsc.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            }
        }

        private async Task GetCategory()
        {
            try
            {
                mCategories = await DependencyService.Get<IProfileRepository>().GetCategory();
                if (mCategories != null || mCategories.Count > 0)
                {
                    pkCategory.ItemsSource = mCategories.Select(x => x.Name).ToList();
                }

                if (pkCategory.SelectedItem != null)
                {
                    var categoryId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.CategoryId;
                    await GetSubCategoryByCategoryId(categoryId);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GetCategory: " + ex.Message);
            }
        }

        private async Task GetSubCategoryByCategoryId(string categoryId)
        {
            try
            {
                if (!Common.EmptyFiels(categoryId))
                {
                    mSubCategories = await DependencyService.Get<IProfileRepository>().GetSubCategory(categoryId);
                    if (mSubCategories != null)
                    {
                        pkSubCategory.ItemsSource = mSubCategories.Select(x => x.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GetSubCategoryByCategoryId: " + ex.Message);
            }
        }

        private async Task CreateSubCategory(bool isNewSubCategory = false)
        {
            try
            {
                string CategoryId = "";

                if (pkCategory.SelectedIndex > -1)
                {
                    mCategories = await DependencyService.Get<IProfileRepository>().GetCategory();
                    var mCategory = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault();
                    if (mCategory != null)
                    {
                        CategoryId = mCategory.CategoryId;
                    }
                }

                if (isNewSubCategory)
                {
                    if (selectedSubCategory != null && selectedSubCategory.Count > 0)
                    {
                        foreach (var subCategory in selectedSubCategory)
                        {
                            if (!Common.EmptyFiels(subCategory) && !Common.EmptyFiels(CategoryId))
                            {
                                await DependencyService.Get<IProfileRepository>().CreateSubCategoryByCategoryId(subCategory, CategoryId);
                            }
                        }

                    }
                }
                else
                {
                    if (!Common.EmptyFiels(txtOtherSubCategory.Text) && !Common.EmptyFiels(CategoryId))
                    {
                        await DependencyService.Get<IProfileRepository>().CreateSubCategoryByCategoryId(txtOtherSubCategory.Text, CategoryId);
                    }
                }

                await GetSubCategoryByCategoryId(CategoryId);

                if (mSubCategories != null)
                {
                    if (selectedSubCategory.Where(x => x == txtOtherSubCategory.Text).Count() == 0)
                    {
                        selectedSubCategory = mSubCategories.Select(x => x.Name).ToList();
                        pkSubCategory.ItemsSource = mSubCategories.Select(x => x.Name).ToList();
                        pkSubCategory.SelectedItem = txtOtherSubCategory.Text;
                        txtOtherSubCategory.Text = string.Empty;
                    }

                    wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/CreateSubCategory: " + ex.Message);
            }
        }

        private async Task GetCountries()
        {
            try
            {
                if (Common.mCountries == null || Common.mCountries.Count == 0)
                {
                    Common.mCountries = await DependencyService.Get<IProfileRepository>().GetCountry();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GetCountries: " + ex.Message);
            }
        }

        private async Task BindSellerDetails()
        {
            try
            {
                mSellerDetail = await DependencyService.Get<ISellerRepository>().GetSellerDetails(SellerId);
                if (mSellerDetail != null)
                {
                    #region [ User Details ]
                    StkSellerId.IsVisible = true;
                    lblSellerId.Text = mSellerDetail.SellerNo;
                    txtFullName.Text = mSellerDetail.FullName;
                    txtEmail.Text = mSellerDetail.Email;
                    txtPhoneNumber.Text = mSellerDetail.PhoneNumber;
                    if (!Common.EmptyFiels((string)mSellerDetail.AlternativePhoneNumber))
                    {
                        txtAltPhoneNumber.Text = (string)mSellerDetail.AlternativePhoneNumber;
                    }
                    #endregion

                    #region [ Billing Address ]
                    txtBuildingNumber.Text = mSellerDetail.Building;
                    txtStreet.Text = mSellerDetail.Street;
                    txtCity.Text = mSellerDetail.City;
                    txtState.Text = mSellerDetail.State;
                    txtPinCode.Text = mSellerDetail.PinCode;
                    txtLandmark.Text = mSellerDetail.Landmark;
                    pkNationality.Text = mSellerDetail.Nationality;
                    #endregion

                    #region [ Company Profile ]

                    if (selectedSubCategory != null)
                    {
                        selectedSubCategory = mSellerDetail.CompanyProfile.SubCategories;
                        wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                    }

                    if (mSellerDetail.CompanyProfile.Category != null)
                    {
                        var category = mCategories.Where(x => x.Name == mSellerDetail.CompanyProfile.Category).FirstOrDefault()?.Name;

                        if (!Common.EmptyFiels(category))
                        {
                            pkCategory.SelectedItem = category;
                        }
                        else
                        {
                            txtOtherCategory.Text = mSellerDetail.CompanyProfile?.Category;
                            //
                            mCategories = await DependencyService.Get<IProfileRepository>().CreateCategory(txtOtherCategory.Text);
                            if (mCategories != null)
                            {
                                var lastAddedCategory = mCategories.Select(x => x.Name).ToList();
                                if (lastAddedCategory.Any(x => x.ToLower().Trim() == txtOtherCategory.Text.ToLower().Trim()))
                                {
                                    pkCategory.ItemsSource = lastAddedCategory.ToList();
                                    pkCategory.SelectedItem = txtOtherCategory.Text;

                                    txtOtherCategory.Text = string.Empty;
                                    await CreateSubCategory(true);
                                }
                            }
                        }
                    }

                    txtDescription.Text = mSellerDetail.CompanyProfile.Description;
                    txtExperience.Text = mSellerDetail.CompanyProfile.Experience;
                    txtSupplyArea.Text = mSellerDetail.CompanyProfile.AreaOfSupply;
                    lblSellerCommission.Text = "" + mSellerDetail.CompanyProfile.CommissionRate + "%";
                    #endregion

                    #region [ Bank Information ]
                    txtGstNumber.Text = mSellerDetail.BankInformation.Gstin.ToUpper();
                    txtPan.Text = mSellerDetail.BankInformation.Pan.ToUpper();
                    txtBankAccount.Text = mSellerDetail.BankInformation.BankAccountNumber;
                    txtBankName.Text = mSellerDetail.BankInformation.Branch;
                    txtIfsc.Text = mSellerDetail.BankInformation.Ifsc;
                    #endregion

                    #region [ Document List ]
                    documentList = mSellerDetail.Documents;
                    AddDocuments();
                    #endregion 
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/BindSellerDetails: " + ex.Message);
            }
        }

        private void AddDocuments()
        {
            try
            {
                if (documentList != null && documentList.Count > 0)
                {
                    lblAttachDocument.IsVisible = false;
                    lstDocument.ItemsSource = documentList.ToList();
                    lstDocument.IsVisible = true;
                }
                else
                {
                    lblAttachDocument.IsVisible = true;
                    lstDocument.ItemsSource = null;
                    lstDocument.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/BindDocumentList: " + ex.Message);
            }
        }
        #endregion

        #region [ Validations ]
        private bool ValidatePassword()
        {
            bool isValid = false;
            if (Common.EmptyFiels(SellerId))
            {
                if (Common.EmptyFiels(txtPassword.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Password);
                }
                else if (!Common.IsValidPassword(txtPassword.Text))
                {
                    App.Current.MainPage.DisplayAlert(Constraints.Alert, String.Format("The {0} must be at least {1} characters long and should have atleast one capital leter, special character ({2}) and digit.", "Password", 8, "#$^+=!*()@%&"), Constraints.Ok);
                }
                else
                {
                    isValid = true;
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private bool ValidateUserDetails()
        {
            bool isValid = false;
            if (Common.EmptyFiels(txtEmail.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Email);
            }
            else if (!txtEmail.Text.IsValidEmail())
            {
                Common.DisplayErrorMessage(Constraints.InValid_Email);
            }
            else if (Common.EmptyFiels(txtPhoneNumber.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_PhoneNumber);
            }
            else if (!txtPhoneNumber.Text.IsValidPhone())
            {
                Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
            }
            else if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
            {
                if (!Common.IsValidPhone(txtAltPhoneNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.InValid_AltNumber);
                }
                else if (txtAltPhoneNumber.Text == txtPhoneNumber.Text)
                {
                    Common.DisplayErrorMessage(Constraints.Same_Phone_AltPhone_Number);
                }
                else
                {
                    isValid = true;
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private bool ValidateCompanyDetails()
        {
            bool isValid = false;

            if (Common.EmptyFiels(txtDescription.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Description);
            }
            else if (pkCategory.SelectedIndex == -1 && Common.EmptyFiels(txtOtherCategory.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Category);
            }
            else if (selectedSubCategory != null && selectedSubCategory.Count == 0 && pkSubCategory.SelectedIndex == -1)
            {
                Common.DisplayErrorMessage(Constraints.Required_Subcategory);
            }
            else if (selectedSubCategory != null && selectedSubCategory.Count == 0)
            {
                Common.DisplayErrorMessage(Constraints.Required_Subcategory);
            }
            else if (Common.EmptyFiels(txtExperience.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Experience);
            }
            else if (Common.EmptyFiels(txtSupplyArea.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_SupplyArea);
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private bool ValidateBankInfo()
        {
            bool isValid = false;
            if (Common.EmptyFiels(txtGstNumber.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_GST);
            }
            else if (!Common.IsValidGSTPIN(txtGstNumber.Text.ToUpper()))
            {
                Common.DisplayErrorMessage(Constraints.InValid_GST);
            }
            else if (Common.EmptyFiels(txtPan.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_PAN);
            }
            else if (!Common.IsValidPAN(txtPan.Text.ToUpper()))
            {
                Common.DisplayErrorMessage(Constraints.InValid_PAN);
            }
            else if (Common.EmptyFiels(txtBankAccount.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Bank_Account);
            }
            else if (Common.EmptyFiels(txtBankName.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Bank_Name);
            }
            else if (Common.EmptyFiels(txtIfsc.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_IFSC);
            }
            else if (!isChecked)
            {
                Common.DisplayErrorMessage(Constraints.Agree_T_C);
            }
            else if (Common.IsValidGSTPIN(txtGstNumber.Text.ToUpper()) && Common.IsValidPAN(txtPan.Text.ToUpper()))
            {
                string panFromGSTIN = txtGstNumber.Text.Substring(2, 10);
                if (panFromGSTIN.ToUpper() != txtPan.Text.ToUpper())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PAN_GSTIN);
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtFullName.Text) || Common.EmptyFiels(txtEmail.Text) || Common.EmptyFiels(txtPassword.Text)
                        || Common.EmptyFiels(txtPhoneNumber.Text) || Common.EmptyFiels(txtDescription.Text)
                        || pkCategory.SelectedIndex == -1 || selectedSubCategory == null
                        || Common.EmptyFiels(txtExperience.Text) || Common.EmptyFiels(txtSupplyArea.Text)
                        || Common.EmptyFiels(txtGstNumber.Text) || Common.EmptyFiels(txtPan.Text)
                        || Common.EmptyFiels(txtBankAccount.Text) || Common.EmptyFiels(txtBankName.Text)
                        || Common.EmptyFiels(txtIfsc.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_FullName);
                }
                else if (!ValidatePassword())
                {
                    isValid = false;
                }
                else if (!ValidateUserDetails())
                {
                    isValid = false;
                }
                else if (!BillingAddressValidations())
                {
                    isValid = false;
                }
                else if (!ValidateCompanyDetails())
                {
                    isValid = false;
                }
                else if (!ValidateBankInfo())
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/Validations: " + ex.Message);
            }
            return isValid;
        }

        private bool BillingAddressValidations()
        {
            bool isValid = false;

            try
            {
                if (Common.EmptyFiels(txtBuildingNumber.Text) || Common.EmptyFiels(txtStreet.Text)
        || Common.EmptyFiels(txtCity.Text) || Common.EmptyFiels(txtState.Text)
        || Common.EmptyFiels(pkNationality.Text) || Common.EmptyFiels(txtPinCode.Text)
        || Common.EmptyFiels(txtLandmark.Text))
                {
                    RequiredBillingFields();
                    if (Common.EmptyFiels(txtBuildingNumber.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_BuildingNumber);
                    }
                    else if (Common.EmptyFiels(txtStreet.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_Street);
                    }
                    else if (Common.EmptyFiels(txtCity.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_City);
                    }
                    else if (Common.EmptyFiels(txtState.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_State);
                    }
                    else if (Common.EmptyFiels(pkNationality.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_Nationality);
                    }
                    else if (Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower()).Count() == 0)
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Nationality);
                    }
                    else if (Common.EmptyFiels(txtLandmark.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_Landmark);
                    }
                    else if (Common.EmptyFiels(txtPinCode.Text))
                    {
                        isValid = PinCodeValidation().Result;
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                else
                {
                    isValid = true;

                }

                if (isValid)
                {
                    BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxState.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/BillingAddressValidations: " + ex.Message);
            }

            return isValid;
        }

        private void HasUpdateProfileDetail()
        {
            try
            {
                bool isUpdate = false;

                if (!Common.EmptyFiels(SellerId))
                {
                    if (mSellerDetail == null)
                        isUpdate = true;

                    if (mSellerDetail.FullName != txtFullName.Text)
                        isUpdate = true;
                    else if (mSellerDetail.Email != txtEmail.Text)
                        isUpdate = true;
                    else if (mSellerDetail.PhoneNumber != txtPhoneNumber.Text)
                        isUpdate = true;
                    else if (mSellerDetail.AlternativePhoneNumber != txtAltPhoneNumber.Text)
                        isUpdate = true;
                    else if (mSellerDetail.CompanyProfile.Description != txtDescription.Text)
                        isUpdate = true;
                    else if (mSellerDetail.CompanyProfile.Experience != txtExperience.Text)
                        isUpdate = true;
                    else if (mSellerDetail.CompanyProfile.AreaOfSupply != txtSupplyArea.Text)
                        isUpdate = true;
                    else if (mSellerDetail.BankInformation.Gstin.ToUpper() != txtGstNumber.Text.ToUpper())
                        isUpdate = true;
                    else if (mSellerDetail.BankInformation.Pan.ToUpper() != txtPan.Text.ToUpper())
                        isUpdate = true;
                    else if (mSellerDetail.BankInformation.BankAccountNumber != txtBankAccount.Text)
                        isUpdate = true;
                    else if (mSellerDetail.BankInformation.Branch != txtBankName.Text)
                        isUpdate = true;
                    else if (mSellerDetail.BankInformation.Ifsc != txtIfsc.Text)
                        isUpdate = true;
                    else if (mSellerDetail.Documents != documentList)
                        isUpdate = true;
                    else if (mSellerDetail.CompanyProfile.Category != (string)pkCategory.SelectedItem)
                        isUpdate = true;
                    else if (mSellerDetail.CompanyProfile.SubCategories != selectedSubCategory)
                        isUpdate = true;
                    else if (mSellerDetail.Building != txtBuildingNumber.Text)
                        isUpdate = true;
                    else if (mSellerDetail.Street != txtStreet.Text)
                        isUpdate = true;
                    else if (mSellerDetail.City != txtCity.Text)
                        isUpdate = true;
                    else if (mSellerDetail.State != txtState.Text)
                        isUpdate = true;
                    else if (mSellerDetail.PinCode != txtPinCode.Text)
                        isUpdate = true;
                    else if (mSellerDetail.Nationality != pkNationality.Text)
                        isUpdate = true;
                    else if (mSellerDetail.Landmark != txtLandmark.Text)
                        isUpdate = true;
                    else if (isDeleteSubCategory)
                        isUpdate = true;
                    else if (isUpdatPhoto)
                        isUpdate = true;
                    else if (isChecked)
                        isUpdate = true;
                    else
                        isUpdate = false;
                }
                else
                {
                    isUpdate = true;
                }

                BtnSubmit.IsEnabled = isUpdate;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/HasUpdateProfileDetail: " + ex.Message);
            }
        }

        private void RequiredBillingFields()
        {
            try
            {
                if (Common.EmptyFiels(txtBuildingNumber.Text))
                {
                    BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtStreet.Text))
                {
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtCity.Text))
                {
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtState.Text))
                {
                    BoxState.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPinCode.Text))
                {
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(pkNationality.Text))
                {
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtLandmark.Text))
                {
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/RequiredBillingFields: " + ex.Message);
            }
        }

        private void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPassword.Text))
                {
                    BoxPassword.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtEmail.Text))
                {
                    BoxEmail.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtDescription.Text))
                {
                    BoxDescription.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (pkCategory.SelectedIndex == -1)
                {
                    BoxCategory.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (selectedSubCategory.Count == 0 && pkSubCategory.SelectedIndex == -1)
                {
                    BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtExperience.Text))
                {
                    BoxExperience.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtSupplyArea.Text))
                {
                    BoxSupplyArea.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtGstNumber.Text))
                {
                    BoxGstNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPan.Text))
                {
                    BoxPan.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtBankAccount.Text))
                {
                    BoxBankAccount.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtBankName.Text))
                {
                    BoxBankName.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtIfsc.Text))
                {
                    BoxIfsc.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(SellerId))
                {
                    RequiredBillingFields();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/RequiredFields: " + ex.Message);
            }
        }

        private void UnfocussedFields(Entry entry = null, ExtAutoSuggestBox autoSuggestBox = null, Editor editor = null, Picker picker = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "FullName")
                    {
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Password")
                    {
                        BoxPassword.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Email")
                    {
                        BoxEmail.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Experience")
                    {
                        BoxExperience.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "SupplyArea")
                    {
                        BoxSupplyArea.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "GstNumber")
                    {
                        BoxGstNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Pan")
                    {
                        BoxPan.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BankAccount")
                    {
                        BoxBankAccount.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BankName")
                    {
                        BoxBankName.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "IFSC")
                    {
                        BoxIfsc.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BABuildingNumber")
                    {
                        BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BAStreet")
                    {
                        BoxStreet.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BACity")
                    {
                        BoxCity.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BAState")
                    {
                        BoxState.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BAPincode")
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BALandmark")
                    {
                        BoxLandmark.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (editor != null)
                {
                    if (editor.ClassId == "Description")
                    {
                        BoxDescription.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (picker != null)
                {
                    if (picker.ClassId == "Category")
                    {
                        BoxCategory.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (picker.ClassId == "SubCategory")
                    {
                        BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (autoSuggestBox != null)
                {
                    if (autoSuggestBox.ClassId == "BANationality")
                    {
                        BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/UnfocussedFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            try
            {
                #region User Details            
                txtFullName.Text = txtFullName.Text.Trim();
                txtEmail.Text = txtEmail.Text.Trim();
                txtPhoneNumber.Text = txtPhoneNumber.Text.Trim();
                if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    txtAltPhoneNumber.Text = txtAltPhoneNumber.Text.Trim();
                }
                #endregion

                #region Billing Address
                if (!Common.EmptyFiels(txtBuildingNumber.Text))
                    txtBuildingNumber.Text = txtBuildingNumber.Text.Trim();
                if (!Common.EmptyFiels(txtStreet.Text))
                    txtStreet.Text = txtStreet.Text.Trim();
                if (!Common.EmptyFiels(txtCity.Text))
                    txtCity.Text = txtCity.Text.Trim();
                if (!Common.EmptyFiels(txtState.Text))
                    txtState.Text = txtState.Text.Trim();
                if (!Common.EmptyFiels(txtPinCode.Text))
                    txtPinCode.Text = txtPinCode.Text.Trim();
                if (!Common.EmptyFiels(txtLandmark.Text))
                    txtLandmark.Text = txtLandmark.Text.Trim();
                #endregion

                #region Company Profile
                txtDescription.Text = txtDescription.Text.Trim();
                txtExperience.Text = txtExperience.Text.Trim();
                txtSupplyArea.Text = txtSupplyArea.Text.Trim();
                #endregion

                #region Bank Information
                txtGstNumber.Text = txtGstNumber.Text.ToUpper().Trim();
                txtPan.Text = txtPan.Text.ToUpper().Trim();
                txtBankAccount.Text = txtBankAccount.Text.Trim();
                txtBankName.Text = txtBankName.Text.Trim();
                txtIfsc.Text = txtIfsc.Text.Trim();
                #endregion
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/FieldsTrim: " + ex.Message);
            }
        }

        private async Task<bool> PinCodeValidation()
        {
            bool isValid = false;
            try
            {
                if (!Common.EmptyFiels(txtPinCode.Text))
                {
                    txtPinCode.Text = txtPinCode.Text.Trim();
                    isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(txtPinCode.Text);
                    if (isValid)
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_PinCode);
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/PinCodeValidation: " + ex.Message);
            }
            return isValid;
        }
        #endregion

        #region [ Update Profile Process ]
        private Model.Request.UpdateSeller FillUpdateSeller()
        {
            UpdateSeller mUpdateSeller = new UpdateSeller();
            try
            {
                #region User Details            
                mUpdateSeller.UserId = SellerId;
                mUpdateSeller.FullName = txtFullName.Text;
                mUpdateSeller.Email = txtEmail.Text;
                mUpdateSeller.PhoneNumber = txtPhoneNumber.Text;
                if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    mUpdateSeller.AlternativePhoneNumber = txtAltPhoneNumber.Text;
                }
                #endregion

                #region Billing Address                
                mUpdateSeller.Building = txtBuildingNumber.Text;
                mUpdateSeller.Street = txtStreet.Text;
                mUpdateSeller.City = txtCity.Text;
                mUpdateSeller.State = txtState.Text;
                mUpdateSeller.PinCode = txtPinCode.Text;
                mUpdateSeller.Landmark = txtLandmark.Text;
                mUpdateSeller.CountryId = (int)(Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);
                #endregion

                #region Company Profile
                mUpdateSeller.Description = txtDescription.Text;
                mUpdateSeller.Experience = txtExperience.Text;
                mUpdateSeller.AreaOfSupply = txtSupplyArea.Text;

                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mUpdateSeller.Category = txtOtherCategory.Text;
                }
                else
                {
                    mUpdateSeller.Category = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.Name;
                }

                if (selectedSubCategory != null)
                {
                    mUpdateSeller.SubCategories = selectedSubCategory;
                }
                #endregion

                #region Bank Information
                mUpdateSeller.Gstin = txtGstNumber.Text.ToUpper();
                mUpdateSeller.Pan = txtPan.Text.ToUpper();
                mUpdateSeller.BankAccountNumber = txtBankAccount.Text;
                mUpdateSeller.Branch = txtBankName.Text;
                mUpdateSeller.Ifsc = txtIfsc.Text;
                #endregion

                #region Document List
                mUpdateSeller.Documents = documentList;
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    ErrorMessage = "AddSellerView/FillProfileDetails: " + ex.Message;
                }
                else
                {
                    ErrorMessage = Constraints.Something_Wrong;
                }
                return null;
            }

            return mUpdateSeller;
        }

        private Model.Request.CreateSeller FillCreateSeller()
        {
            CreateSeller mCreateSeller = new CreateSeller();
            try
            {

                #region User Details            
                mCreateSeller.FullName = txtFullName.Text;
                mCreateSeller.Password = txtPassword.Text;
                mCreateSeller.Email = txtEmail.Text;
                mCreateSeller.PhoneNumber = txtPhoneNumber.Text;
                if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    mCreateSeller.AlternativePhoneNumber = txtAltPhoneNumber.Text;
                }
                #endregion

                #region Billing Address              
                mCreateSeller.Building = txtBuildingNumber.Text;
                mCreateSeller.Street = txtStreet.Text;
                mCreateSeller.City = txtCity.Text;
                mCreateSeller.State = txtState.Text;
                mCreateSeller.PinCode = txtPinCode.Text;
                mCreateSeller.Landmark = txtLandmark.Text;
                mCreateSeller.CountryId = (int)(Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);
                #endregion

                #region Company Profile
                mCreateSeller.Description = txtDescription.Text;
                mCreateSeller.Experience = txtExperience.Text;
                mCreateSeller.AreaOfSupply = txtSupplyArea.Text;

                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mCreateSeller.Category = txtOtherCategory.Text;
                }
                else
                {
                    mCreateSeller.Category = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.Name;
                }

                if (selectedSubCategory != null)
                {
                    mCreateSeller.SubCategories = selectedSubCategory;
                }
                #endregion

                #region Bank Information
                mCreateSeller.Gstin = txtGstNumber.Text.ToUpper();
                mCreateSeller.Pan = txtPan.Text.ToUpper();
                mCreateSeller.BankAccountNumber = txtBankAccount.Text;
                mCreateSeller.Branch = txtBankName.Text;
                mCreateSeller.Ifsc = txtIfsc.Text;
                #endregion

                #region Document List
                mCreateSeller.Documents = documentList;
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    ErrorMessage = "AddSellerView/FillProfileDetails: " + ex.Message;
                }
                else
                {
                    ErrorMessage = Constraints.Something_Wrong;
                }
                return null;
            }

            return mCreateSeller;
        }


        private async Task UpdateOrCreateSeller()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    FieldsTrim();

                    var mResponse = new Response();
                    if (Common.EmptyFiels(SellerId))
                    {
                        var mCreateSellerDetails = FillCreateSeller();
                        if (mCreateSellerDetails != null)
                        {
                            mResponse = await sellerManagementAPI.CreateSeller(mCreateSellerDetails);
                            if (mResponse != null && mResponse.Succeeded)
                            {
                                SuccessfullUpdate(mResponse.Message);
                                await Navigation.PushAsync(new MainTabbedPage(Constraints.Str_Manage));
                                BtnSubmit.IsEnabled = false;
                            }
                            else
                            {
                                if (mResponse == null)
                                    return;

                                Common.DisplayErrorMessage(mResponse.Message);
                            }
                        }
                        else
                        {
                            Common.DisplayErrorMessage(ErrorMessage);
                        }
                    }
                    else
                    {
                        var mSellerDetails = FillUpdateSeller();
                        if (mSellerDetails != null)
                        {
                            mResponse = await sellerManagementAPI.UpdateSeller(mSellerDetails);
                            if (mResponse != null && mResponse.Succeeded)
                            {
                                await BindSellerDetails();

                                isDeleteSubCategory = false;
                                isUpdatPhoto = false;
                                var updateId = mResponse.Data;
                                if (updateId != null)
                                {
                                    SuccessfullUpdate(mResponse.Message);
                                    BtnSubmit.IsEnabled = false;
                                }
                            }
                            else
                            {
                                if (mResponse == null)
                                    return;

                                Common.DisplayErrorMessage(mResponse.Message);
                            }
                        }
                        else
                        {
                            Common.DisplayErrorMessage(ErrorMessage);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/UpdateProfile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void SuccessfullUpdate(string MessageString)
        {
            try
            {
                var successPopup = new Popup.SuccessPopup(MessageString);
                await PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/SuccessfullUpdate: " + ex.Message);
            }
        }

        private void ClearBillingAddress()
        {
            txtBuildingNumber.Text = string.Empty;
            txtStreet.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            txtLandmark.Text = string.Empty;
            pkNationality.Text = string.Empty;
        }
        #endregion
        #endregion

        #region [ Events ]

        #region [ Header Navigation ]    
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            if (!Common.EmptyFiels(SellerId))
            {
                Navigation.PopAsync();
            }
            else
            {
                Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
            }
        }
        #endregion

        private async void ImgUplodeDocument_Clicked(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(imageButton: ImgUplodeDocument);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                await FileSelection.FilePickup();
                relativeDocumentPath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

                if (!Common.EmptyFiels(relativeDocumentPath))
                {
                    if (documentList == null)
                        documentList = new List<string>();
                    isUpdatPhoto = true;
                    documentList.Add(relativeDocumentPath);
                    AddDocuments();
                    HasUpdateProfileDetail();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/ImgUplodeDocument_Clicked: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void ImgPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = Convert.ToString(imgEye.Source).Replace("File: ", string.Empty);
                if (selectedImage == Constraints.Img_Hide)
                {
                    txtPassword.IsPassword = false;
                    imgEye.Source = Constraints.Img_Show;
                }
                else
                {
                    txtPassword.IsPassword = true;
                    imgEye.Source = Constraints.Img_Hide;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/ImgPassword_Tapped: " + ex.Message);
            }
        }

        private void GrdCompanyProfile_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdCompanyProfile.IsVisible == false)
                {
                    GrdCompanyProfile.IsVisible = true;
                    ImgDownDownCompanyProfile.Source = Constraints.Arrow_Up;
                    ScrPrimary.ScrollToAsync(GrdCompnyProfile, ScrollToPosition.Start, true);
                }
                else
                {
                    GrdCompanyProfile.IsVisible = false;
                    ImgDownDownCompanyProfile.Source = Constraints.Arrow_Down;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GrdCompanyProfile_Tapped: " + ex.Message);
            }
        }

        private void GrdBankInfo_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdBankInfo.IsVisible == false)
                {
                    GrdBankInfo.IsVisible = true;
                    ImgDropDownBankInfo.Source = Constraints.Arrow_Up;
                    ScrPrimary.ScrollToAsync(GrdBankInformation, ScrollToPosition.Start, true);
                }
                else
                {
                    GrdBankInfo.IsVisible = false;
                    ImgDropDownBankInfo.Source = Constraints.Arrow_Down;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GrdBankInfo_Tapped: " + ex.Message);
            }
        }

        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnSubmit);
            await UpdateOrCreateSeller();
        }

        private void StkAcceptTerm_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgCheck.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
                {
                    isChecked = false;
                    imgCheck.Source = Constraints.CheckBox_UnChecked;
                }
                else
                {
                    isChecked = true;
                    imgCheck.Source = Constraints.CheckBox_Checked;
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/StkAcceptTerm_Tapped: " + ex.Message);
            }
        }

        #region [ AutoSuggestBox-Country ]
        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    if (isFirstLoad || i < 2)
                    {
                        isFirstLoad = false;
                        pkNationality.IsSuggestionListOpen = false;
                        i++;
                        return;
                    }
                }

                if (mCountriesData == null)
                    mCountriesData = new ObservableCollection<string>();

                mCountriesData.Clear();
                if (!string.IsNullOrEmpty(pkNationality.Text))
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Where(x => x.Name.ToLower().Contains(pkNationality.Text.ToLower())).Select(x => x.Name));
                }
                else
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Select(x => x.Name));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/AutoSuggestBox_TextChanged: " + ex.Message);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                if (e.ChosenSuggestion != null)
                {
                    pkNationality.Text = e.ChosenSuggestion.ToString();
                }
                else
                {
                    // User hit Enter from the search box. Use args.QueryText to determine what to do.
                    pkNationality.Unfocus();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/AutoSuggestBox_QuerySubmitted: " + ex.Message);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }
        #endregion

        #region [ Category-SubCategory ]
        private async void pkCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkCategory.SelectedIndex > -1 && pkCategory.SelectedItem != null)
                {
                    var newCategoryId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).Select(x => x.CategoryId).FirstOrDefault();
                    if (newCategoryId != null)
                    {
                        await GetSubCategoryByCategoryId(newCategoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/Category_SelectedIndex: " + ex.Message);
            }
        }

        private void pkSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkSubCategory.SelectedIndex > -1 && pkSubCategory.SelectedItem != null)
                {
                    if (selectedSubCategory.Where(x => x == pkSubCategory.SelectedItem.ToString()).Count() == 0)
                    {
                        selectedSubCategory.Add(pkSubCategory.SelectedItem.ToString());
                    }

                    wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/pkSubCategory_SelectedIndexChanged: " + ex.Message);
            }
        }

        private void BtnDeleteSubCategory_Clicked(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                var subName = (string)btn.BindingContext;
                if (selectedSubCategory != null && selectedSubCategory.Count > 0)
                {
                    selectedSubCategory.Remove(subName);
                    isDeleteSubCategory = true;
                    HasUpdateProfileDetail();
                }

                wlSubCategory.ItemsSource = selectedSubCategory.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/BtnDeleteSubCategory: " + ex.Message);
            }
        }

        private async void txtOtherCategory_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mCategories = await DependencyService.Get<IProfileRepository>().CreateCategory(txtOtherCategory.Text);
                    if (mCategories != null)
                    {
                        selectedSubCategory = new List<string>();
                        wlSubCategory.ItemsSource = selectedSubCategory.ToList();

                        var lastAddedCategory = mCategories.Select(x => x.Name).ToList();
                        if (lastAddedCategory.Any(x => x.ToLower().Trim() == txtOtherCategory.Text.ToLower().Trim()))
                        {
                            pkCategory.ItemsSource = lastAddedCategory.ToList();
                            pkCategory.SelectedItem = txtOtherCategory.Text;

                            txtOtherCategory.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/txtOtherCategory_Completed: " + ex.Message);
            }
        }

        private async void txtOtherSubCategory_Unfocused(object sender, FocusEventArgs e)
        {
            await CreateSubCategory();
        }
        #endregion

        #region [ Billing Address ]
        private void GrdBilling_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdBilling.IsVisible == false)
                {
                    GrdBilling.IsVisible = true;
                    ImgDropDownBilling.Source = Constraints.Arrow_Up;
                    ScrPrimary.ScrollToAsync(GrdBillingAddress, ScrollToPosition.Start, true);
                }
                else
                {
                    GrdBilling.IsVisible = false;
                    ImgDropDownBilling.Source = Constraints.Arrow_Down;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GrdBilling_Tapped: " + ex.Message);
            }
        }


        private void BillingAddressEntry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                UnfocussedFields(entry: entry);
            }
        }

        private async void BillingPincode_Unfocused(object sender, FocusEventArgs e)
        {
            await PinCodeValidation();
        }
        #endregion

        #region [ Unfocus ]
        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                UnfocussedFields(entry: entry);
            }
        }

        private void AutoSuggestBox_Unfocused(object sender, FocusEventArgs e)
        {
            var autoSuggestBox = (ExtAutoSuggestBox)sender;
            if (!Common.EmptyFiels(autoSuggestBox.Text))
            {
                UnfocussedFields(autoSuggestBox: autoSuggestBox);
            }
        }

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            var editor = (Editor)sender;
            if (!Common.EmptyFiels(editor.Text))
            {
                UnfocussedFields(editor: editor);
            }
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedIndex != -1)
            {
                UnfocussedFields(picker: picker);
            }
        }
        #endregion

        #endregion

        private async void ImgDocument_Clicked(object sender, EventArgs e)
        {
            var imgButton = (ImageButton)sender;
            try
            {
                var url = imgButton.BindingContext as string;
                await GenerateWebView.GenerateView(url);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/ImgDocument_Clicked: " + ex.Message);
            }
        }
    }
}