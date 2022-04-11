using Xamarin.Forms;

namespace aptdealzMExecutiveMobile.Utility
{
    public class Constraints
    {
        #region [ KeyWords ]
        public const string Str_Home = "Home";
        public const string Str_AddSeller = "AddSeller";
        public const string Str_ManageSeller = "ManageSeller";
        public const string Str_Manage = "Manage";
        public const string Str_Account = "Account";
        public const string Str_About = "About";
        public const string Str_Support = "Support";
        public const string Str_FAQHelp = "FAQHelp";
        public const string Str_Settings = "Settings";
        public const string Str_Notifications = "Notifications";
        public const string Str_TokenExpired = "TokenExpired";
        public const string Str_AccountDeactivated = "account is deactivated";
        public const string Str_NotificationCount = "NotificationCount";
        public const string Str_Duplicate = "duplicate";
        public const string THEME_DARKMODE = "DarkModeResources";
        public const string THEME_LIGHTMODE = "LightModeResources";
        #endregion

        #region [ Image ]
        public const string Img_Home = "iconHome.png";
        public const string Img_Home_Dark = "iconHomeWhite.png";
        public const string Img_Home_Active = "iconHomeActive.png";

        public const string Img_AddSeller = "iconAddSellerBlack.png";
        public const string Img_AddSeller_Dark = "iconAddSellerWhite.png";
        public const string Img_AddSeller_Active = "iconAddSellerOrange.png";

        public const string Img_ManageSeller = "iconManageSellerBlack.png";
        public const string Img_ManageSeller_Dark = "iconManageSellerWhite.png";
        public const string Img_ManageSeller_Active = "iconManageSellerActive.png";

        public const string Img_Account = "iconAccount.png";
        public const string Img_Account_Dark = "iconAccountWhite.png";
        public const string Img_Account_Active = "iconAccountActive.png";

        public const string Redio_UnSelected = "iconRadioUnselect.png";
        public const string Redio_Selected = "iconRadioSelect.png";

        public const string Sort_ASC = "iconSortASC.png";
        public const string Sort_ASC_Dark = "iconSortASCWhite.png";
        public const string Sort_DSC = "iconSortDSC.png";
        public const string Sort_DSC_Dark = "iconSortDSCWhite.png";

        public const string CheckBox_UnChecked = "iconUncheck.png";
        public const string CheckBox_Checked = "iconCheck.png";

        public const string Img_Show = "iconShowGray.png";
        public const string Img_Hide = "iconHide.png";

        public const string Arrow_Right = "iconRightArrow.png";
        public const string Arrow_Down = "iconDownArrow.png";
        public const string Arrow_Up = "iconUpArrow.png";

        public const string ImgMusic = "iconMusic.png";
        public const string ImgVideo = "iconVideo.png";
        public const string ImgFile = "iconFiles2.png";

        public const string ImgUserAccount = "iconUserAccount.png";
        public const string ImgUploadImage = "imgUploadImage.png";

        public const string ImgSad = "iconSad.png";
        public const string ImgSmile = "iconSmile.png";

        public const string ImgContact = "imgContact.jpg";

        public const string Img_SwitchOff = "iconSwitchOff.png";
        public const string Img_SwitchOn = "iconSwitchOn.png";

        public const string Img_GreenArrowDown = "iconGreenDownArrow.png";
        public const string Img_GreenArrowUp = "iconGreenUpArrow.png";
        #endregion

        #region [ Success ]
        public const string OTPSent = "OTP Verification Code Sent Successfully";
        public const string InstantVerification = "you don't get any code, it is instant verification. Please try to login with email address";
        #endregion

        #region [ Error Message ]
        public static string Session_Expired = "Session expired, Please login again!";
        public static string Something_Wrong = "Something went wrong!";
        public static string Something_Wrong_Server = "Something went wrong from server!";
        public static string ServiceUnavailable = "Service Unavailable, Try again later!";
        public static string Number_was_null = "Number was null or white space";
        public static string Phone_Dialer_Not_Support = "Phone Dialer is not supported on this device";
        #endregion

        #region [ Alert Title ]
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Ok = "OK";
        public const string TryAgain = "Try Again";
        public const string Cancel = "Cancel";
        public const string UploadPicture = "Upload Picture";
        public const string TakePhoto = "Take Photo";
        public const string ChooseFromLibrary = "Choose From Library";
        public const string NoCamera = "No Camera";
        public const string Alert = "Alert!";
        public const string Loading = "Loading...";
        public const string Logout = "Logout";
        public const string LoginTrackingString = "00000000-0000-0000-0000-000000000000";
        #endregion

        #region [ Alert Message ]  
        public const string AreYouSureWantLogout = "Are you sure want to logout?";
        public const string AreYouSureWantDelete = "Are you sure want to delete?";

        public const string DoYouWantToExit = "Do you really want to exit?";
        public const string NoInternetConnection = "No internet connection!";
        public const string UnableTakePhoto = "Unable to take photo!";
        public const string NoCameraAwailable = "No camera awailable!";
        public const string PermissionDenied = "Permission denied!";
        public const string PhotosNotSupported = "The Photo is not supported!";
        public const string PermissionNotGrantedPhotos = "Permission is not granted to photos!";
        public const string NeedStoragePermissionAccessYourPhotos = "Need storage permission to access your photos!";
        public const string PlsCheckInternetConncetion = "Please check internet connection to use App.";
        public const string CouldNotSentOTP = "Could not send Verification Code to the given number!";

        public const string Required_All = "Selected fields are required!";
        public const string Required_Email_Phone = "Please enter email address or phone number!";
        public const string Required_Email = "Please enter email address!";
        public const string Required_VerificationCode = "Please enter verification code!";
        public const string Required_FullName = "Please enter full name!";
        public const string Required_Password = "Please enter password!";
        public const string Required_PhoneNumber = "Please enter phone number!";
        public const string Required_BuildingNumber = "Please enter building number!";
        public const string Required_Street = "Please enter street!";
        public const string Required_City = "Please enter city!";
        public const string Required_State = "Please enter state!";
        public const string Required_Nationality = "Please enter country!";
        public const string Required_PinCode = "Please enter pincode!";
        public const string Required_Landmark = "Please enter landmark!";
        public const string Required_Description = "Please enter description!";
        public const string Required_Experience = "Please enter experience!";
        public const string Required_SupplyArea = "Please enter Area of supply!";
        public const string Required_GST = "Please enter GST number!";
        public const string Required_PAN = "Please enter PAN number!";
        public const string Required_Bank_Account = "Please enter bank Account number!";
        public const string Required_Bank_Name = "Please enter bank name!";
        public const string Required_IFSC = "Please enter IFSC code!";
        public const string Required_Category = "Please enter category!";
        public const string Required_Subcategory = "Please enter sub category!";

        public const string InValid_Email = "Please enter valid email address!";
        public const string InValid_PhoneNumber = "Please enter valid phone number!";
        public const string InValid_AltNumber = "Please enter valid alternate phone number!";
        public const string InValid_OTP = "Verification code is invalid! Try again!";
        public const string InValid_Nationality = "Please enter valid country!";
        public const string InValid_State = "Please enter valid state!";

        public const string InValid_Pincode = "Please enter valid pincode!";
        public const string InValid_DeliveryPinCode = "Please enter valid delivery location pin code!";
        public const string InValid_BillingPinCode = "Please enter valid billing address pin code!";
        public const string InValid_ShillingPinCode = "Please enter valid shipping address pin code!";

        public const string InValid_GST = "Please enter valid GST number!";
        public const string InValid_PAN = "Please enter valid PAN number!";
        public const string InValid_PAN_GSTIN = "Please enter valid GST number or PAN number!";

        public const string Same_New_Confirm_Password = "New and Confirm password should be same!";
        public const string Same_Phone_AltPhone_Number = "Please enter different alternative number!";
        public const string Agree_T_C = "Please check box for Terms & Conditions.";
        #endregion



    }
}
