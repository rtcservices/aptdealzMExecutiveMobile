using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Extention;
using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using Rg.Plugins.Popup.Services;
using System;
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
    public partial class AccountView : ContentView, INotifyPropertyChanged
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

        #region [ Objects ]     
        private ProfileAPI profileAPI;
        private ExecutiveDetails mExecutiveDetails;
        private string relativePath;
        bool isFirstLoad = true;
        private bool isUpdatPhoto = false;
        private bool isUpdateProfile = false;
        #endregion

        #region [ Constructor ]
        public AccountView()
        {
            try
            {
                InitializeComponent();
                txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtStreet.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtCity.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtState.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                BtnUpdate.IsEnabled = false;
                BindProperties();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]   
        #region [ Get / Bind Data ]
        private async void BindProperties()
        {
            try
            {
                profileAPI = new ProfileAPI();
                if (Common.mCountries == null || Common.mCountries.Count == 0)
                    await GetCountries();

                await GetProfile();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProperties: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task GetCountries()
        {
            try
            {
                Common.mCountries = await profileAPI.GetCountry();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetCountries: " + ex.Message);
            }
        }

        private async Task GetProfile()
        {
            try
            {
                if (Common.mExecutiveDetails == null || Common.EmptyFiels(Common.mExecutiveDetails.UserId) || isUpdateProfile)
                {
                    mExecutiveDetails = await DependencyService.Get<IProfileRepository>().GetMyProfileData();
                }
                else
                {
                    mExecutiveDetails = Common.mExecutiveDetails;
                }

                GetProfileDetails(mExecutiveDetails);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetProfile: " + ex.Message);
            }
        }

        private void GetProfileDetails(ExecutiveDetails mExecutiveDetails)
        {
            try
            {
                txtFullName.Text = mExecutiveDetails.FullName;
                txtEmailAddress.Text = mExecutiveDetails.Email;
                txtPhoneNumber.Text = mExecutiveDetails.PhoneNumber;

                if (!Common.EmptyFiels(mExecutiveDetails.ProfilePhoto))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mExecutiveDetails.ProfilePhoto = baseURL + mExecutiveDetails.ProfilePhoto.Replace(baseURL, "");
                    imgUser.Source = mExecutiveDetails.ProfilePhoto;
                }
                else
                {
                    imgUser.Source = Constraints.ImgUserAccount;
                }
                if (!Common.EmptyFiels(mExecutiveDetails.Building))
                {
                    txtBuildingNumber.Text = mExecutiveDetails.Building;
                }
                if (!Common.EmptyFiels(mExecutiveDetails.Street))
                {
                    txtStreet.Text = mExecutiveDetails.Street;
                }
                if (!Common.EmptyFiels(mExecutiveDetails.City))
                {
                    txtCity.Text = mExecutiveDetails.City;
                }
                if (!Common.EmptyFiels(mExecutiveDetails.State))
                {
                    txtState.Text = mExecutiveDetails.State;
                }
                if (!Common.EmptyFiels(mExecutiveDetails.PinCode))
                {
                    txtPinCode.Text = mExecutiveDetails.PinCode;
                }
                if (mExecutiveDetails.CountryId > 0 && Common.mCountries != null && Common.mCountries.Count() > 0)
                {
                    pkNationality.Text = Common.mCountries.Where(x => x.CountryId == mExecutiveDetails.CountryId).FirstOrDefault().Name;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProfileDetails: " + ex.Message);
            }
        }
        #endregion

        #region [ Validation ]
        private bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtFullName.Text) || Common.EmptyFiels(txtPhoneNumber.Text) || Common.EmptyFiels(pkNationality.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_FullName);
                }
                else if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PhoneNumber);
                }
                else if (!txtPhoneNumber.Text.IsValidPhone())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                }
                else if (Common.EmptyFiels(pkNationality.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Nationality);
                }
                else if (Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower()).Count() == 0)
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Nationality);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Validations: " + ex.Message);
            }
            return isValid;
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
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    isValid = true;
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/PinCodeValidation: " + ex.Message);
            }
            return isValid;
        }

        private void RequiredFields()
        {
            try
            {
                Common.DisplayErrorMessage(Constraints.Required_All);

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(pkNationality.Text))
                {
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/RequiredFields: " + ex.Message);
            }
        }

        private void UnfocussedFields(Entry entry = null, ExtAutoSuggestBox autoSuggestBox = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "FullName")
                    {
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }

                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (autoSuggestBox != null)
                {
                    if (autoSuggestBox.ClassId == "Nationality")
                    {
                        BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UnfocussedFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            try
            {
                txtFullName.Text = txtFullName.Text.Trim();
                txtPhoneNumber.Text = txtPhoneNumber.Text.Trim();
                if (!Common.EmptyFiels(txtBuildingNumber.Text))
                {
                    txtBuildingNumber.Text = txtBuildingNumber.Text.Trim();
                }
                if (!Common.EmptyFiels(txtStreet.Text))
                {
                    txtStreet.Text = txtStreet.Text.Trim();
                }
                if (!Common.EmptyFiels(txtCity.Text))
                {
                    txtCity.Text = txtCity.Text.Trim();
                }
                if (!Common.EmptyFiels(txtState.Text))
                {
                    txtState.Text = txtState.Text.Trim();
                }
                if (!Common.EmptyFiels(txtPinCode.Text))
                {
                    txtPinCode.Text = txtPinCode.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/FieldsTrim: " + ex.Message);
            }
        }

        private void HasUpdateProfileDetail()
        {
            try
            {
                bool isUpdate = false;
                if (mExecutiveDetails == null)
                    isUpdate = true;

                if (mExecutiveDetails.FullName != txtFullName.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.PhoneNumber != txtPhoneNumber.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.Building != txtBuildingNumber.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.Street != txtStreet.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.City != txtCity.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.PinCode != txtPinCode.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.State != txtState.Text)
                    isUpdate = true;
                else if (mExecutiveDetails.CountryId != Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId)
                    isUpdate = true;
                else if (isUpdatPhoto)
                    isUpdate = true;
                else
                    isUpdate = false;

                BtnUpdate.IsEnabled = isUpdate;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/HasUpdateProfileDetail: " + ex.Message);
            }

        }
        #endregion

        #region [ Update ]
        private Model.Request.ExecutiveDetails UpdateProfileDetails()
        {
            try
            {
                //mExecutiveDetails.UserId = mExecutiveDetails.BuyerId;
                mExecutiveDetails.FullName = txtFullName.Text;
                mExecutiveDetails.Email = txtEmailAddress.Text;
                mExecutiveDetails.PhoneNumber = txtPhoneNumber.Text;

                if (!Common.EmptyFiels(relativePath))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mExecutiveDetails.ProfilePhoto = relativePath.Replace(baseURL, "");
                }
                mExecutiveDetails.Building = txtBuildingNumber.Text;
                mExecutiveDetails.Street = txtStreet.Text;
                mExecutiveDetails.City = txtCity.Text;
                if (!Common.EmptyFiels(pkNationality.Text))
                {
                    mExecutiveDetails.CountryId = (int)(Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);
                }
                mExecutiveDetails.State = txtState.Text;
                mExecutiveDetails.PinCode = txtPinCode.Text;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UpdateProfileDetails: " + ex.Message);
            }
            return mExecutiveDetails;
        }

        private async Task UpdateProfile()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    if (!await PinCodeValidation())
                        return;

                    FieldsTrim();
                    var mExecutiveDetails = UpdateProfileDetails();

                    var mResponse = await profileAPI.SaveProfile(mExecutiveDetails);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        isUpdateProfile = true;
                        await GetProfile();

                        var updateId = mResponse.Data;
                        if (updateId != null)
                        {
                            BtnUpdate.IsEnabled = false;
                            var successPopup = new Popup.SuccessPopup(mResponse.Message);
                            await PopupNavigation.Instance.PushAsync(successPopup);
                        }
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);

                        BtnUpdate.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UpdateProfile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
        #endregion

        #region [ Events ]     
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnUpdate);
                    await UpdateProfile();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("AccountView/BtnUpdate_Clicked: " + ex.Message);
                }
            }
        }

        private async void ImgCamera_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(image: ImgCamera);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                ImageConvertion.SelectedImagePath = imgUser;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfilePicture);
                await ImageConvertion.SelectImage();

                if (ImageConvertion.SelectedImageByte != null)
                {
                    relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfilePicture);
                    isUpdatPhoto = true;
                    HasUpdateProfileDetail();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgCamera_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            UnfocussedFields(entry: entry);
        }

        private async void txtPinCode_Unfocused(object sender, FocusEventArgs e)
        {
            await PinCodeValidation();
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                rfView.IsRefreshing = true;
                BtnUpdate.IsEnabled = false;
                BindProperties();
                rfView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/RefreshView_Refreshing: " + ex.Message);
            }
        }

        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
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
                Common.DisplayErrorMessage("AccountView/AutoSuggestBox_TextChanged: " + ex.Message);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
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
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AutoSuggestBox_QuerySubmitted: " + ex.Message);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }

        private void AutoSuggestBox_Unfocused(object sender, FocusEventArgs e)
        {
            var autoSuggestBox = (ExtAutoSuggestBox)sender;
            if (!Common.EmptyFiels(autoSuggestBox.Text))
            {
                UnfocussedFields(autoSuggestBox: autoSuggestBox);
            }
        }

        private async void Logout_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnLogout);
                    await DependencyService.Get<IAuthenticationRepository>().DoLogout();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("AccountView/Logout_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }
        #endregion
    }
}