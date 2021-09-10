using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Services
{
    public class ProfileRepository : IProfileRepository
    {
        ProfileAPI profileAPI = new ProfileAPI();
        CategoryAPI categoryAPI = new CategoryAPI();

        public async Task<List<Country>> GetCountry()
        {
            List<Country> mCountry = new List<Country>();
            try
            {
                mCountry = await profileAPI.GetCountry();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetCountry: " + ex.Message);
            }
            return mCountry;
        }

        public async Task<List<Category>> GetCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = await categoryAPI.GetCategory();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return categories;
        }

        public async Task<List<SubCategory>> GetSubCategory(string CategortyId)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                subCategories = await categoryAPI.GetSubCategory(CategortyId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetSubCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return subCategories;
        }

        public async Task<List<Category>> CreateCategory(string OtherCategory)
        {
            List<Category> categories = new List<Category>();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await categoryAPI.CreateCategory(OtherCategory);
                if (mResponse != null && mResponse.Succeeded)
                {
                    categories = await GetCategory();
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/CreateCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return categories;
        }

        public async Task<List<SubCategory>> CreateSubCategory(string OtherSubCategory, string categoryId)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await categoryAPI.CreateSubCategory(OtherSubCategory, categoryId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    subCategories = await GetSubCategory(categoryId);
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/CreateSubCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return subCategories;
        }

        public async Task CreateSubCategoryByCategoryId(string OtherSubCategory, string categoryId)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await categoryAPI.CreateSubCategory(OtherSubCategory, categoryId);
                if (mResponse != null && mResponse.Succeeded)
                {
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/CreateSubCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task<bool> ValidPincode(string pinCode)
        {
            bool isValid = false;
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                if (Common.IsValidPincode(pinCode))
                {
                    var response = await profileAPI.GetPincodeInfo(pinCode);
                    if (response != null && response.Succeeded)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/ValidPincode: " + ex.Message);
            }
            return isValid;
        }

        public async Task<ExecutiveDetails> GetMyProfileData()
        {
            ExecutiveDetails mExecutiveDetails = new ExecutiveDetails();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await profileAPI.GetMyProfileData();
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mExecutiveDetails = jObject.ToObject<ExecutiveDetails>();
                        Common.mExecutiveDetails = mExecutiveDetails;
                    }
                }
                else
                {
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetMyProfileData: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mExecutiveDetails;
        }

        //public async Task DeactivateAccount()
        //{
        //    try
        //    {
        //        var result = await App.Current.MainPage.DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantDeactivateAccount, Constraints.Yes, Constraints.No);
        //        if (result)
        //        {
        //            ProfileAPI profileAPI = new ProfileAPI();
        //            UserDialogs.Instance.ShowLoading(Constraints.Loading);
        //            var mResponse = await profileAPI.DeactiviateUser(Settings.UserId);
        //            if (mResponse != null && mResponse.Succeeded)
        //            {
        //                Settings.EmailAddress = string.Empty;
        //                Settings.UserToken = string.Empty;
        //                Settings.RefreshToken = string.Empty;
        //                Settings.UserId = string.Empty;
        //                Settings.LoginTrackingKey = string.Empty;
        //                Settings.fcm_token = string.Empty;
        //                MessagingCenter.Unsubscribe<string>(this, "NotificationCount");
        //                App.stoppableTimer.Stop();
        //                App.Current.MainPage = new NavigationPage(new Views.Accounts.LoginPage());
        //            }
        //            else
        //            {
        //                if (mResponse != null)
        //                    Common.DisplayErrorMessage(mResponse.Message);
        //                else
        //                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.DisplayErrorMessage("ProfileRepository/DeactivateAccount: " + ex.Message);
        //    }
        //    finally
        //    {
        //        UserDialogs.Instance.HideLoading();
        //    }
        //}
    }
}
