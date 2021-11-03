namespace aptdealzMExecutiveMobile.Utility
{
    public class EndPointURL
    {
        #region [ Register API ]
        public const string IsUniquePhoneNumber = "api/v{0}/MarkettingExecutiveAuth/authenticate/phone";
        public const string IsUniqueEmail = "api/v{0}/MarkettingExecutiveAuth/authenticate/email";
        #endregion

        #region [ Profile API ]
        public const string GetMyProfileData = "api/v{0}/MarkettingExecutiveManagement/GetMyProfileData";
        public const string SaveProfile = "api/v{0}/MarkettingExecutiveManagement/Update";
        public const string Country = "api/v{0}/Country/Get";
        public const string FileUpload = "api/FileUpload";
        public const string GetPincodeInfo = "api/IndianPincode/GetPincodeInfo/{0}";
        #endregion

        #region [ AuthenticationAPI ]
        public const string SendOtpByEmail = "api/v{0}/MarkettingExecutiveAuth/SendOtpByEmail";
        public const string ExecutiveAuthenticateEmail = "api/v{0}/MarkettingExecutiveAuth/authenticate/email";
        public const string ExecutiveAuthenticatePhone = "api/v{0}/MarkettingExecutiveAuth/authenticate/phone";
        public const string RefreshToken = "api/Account/refresh-token";
        public const string Logout = "api/Account/logout";
        #endregion

        #region [ Category API ]
        public const string Category = "api/v{0}/Category/Get";
        public const string SubCategory = "api/v{0}/SubCategory/Get?CategoryId={1}";
        public const string CreateCategory = "/api/v{0}/Category/Create";
        public const string CreateSubCategory = "api/v{0}/SubCategory/Create";
        #endregion

        #region [ SellerManagement API ]
        public const string ListOfSellers = "api/v{0}/MarkettingExecutiveSellerManagement/ListAllSellers";
        public const string CreateSeller = "api/v{0}/MarkettingExecutiveSellerManagement/Create";
        public const string UpdateSeller = "api/v{0}/MarkettingExecutiveSellerManagement/Update";
        public const string GetSellerDetails = "api/v{0}/MarkettingExecutiveSellerManagement/GetSellerDetails/{1}";
        #endregion

        #region [ Notification API ]
        public const string GetAllNotificationsForUser = "api/v{0}/Notifications/GetAllNotificationsForUser";
        public const string GetNotificationsCountForUser = "api/v{0}/Notifications/GetNotificationsCountForUser";
        public const string SetUserNoficiationAsRead = "api/v{0}/Notifications/SetUserNoficiationAsRead/{1}";
        public const string SetUserNoficiationAsReadAndDelete = "/api/v{0}/Notifications/SetUserNoficiationAsReadAndDelete/{1}";
        #endregion

        #region [ SupportChat API ]
        public const string GetAllMyChat = "api/v{0}/SupportChat/GetAllMyChat";
        public const string SendChatSupportMessage = "api/v{0}/SupportChat/SendChatSupportMessage";
        #endregion


        #region MyRegion
        public const string GetPrivacyPolicyTermsAndConditions = "api/v{0}/AppSettings/GetPrivacyPolicyTermsAndConditions";
        public const string GetFAQ = "api/v{0}/AppSettings/GetFAQ";
        public const string GetAppPromoBar = "api/v{0}/AppSettings/GetAppPromoBar";
        public const string AboutAptdealzMEApp = "api/v{0}/AppSettings/AboutAptdealzMEApp";
        #endregion
    }
}
