using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using Firebase.Auth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(aptdealzMExecutiveMobile.iOS.Service.FirebaseAuthenticator))]

namespace aptdealzMExecutiveMobile.iOS.Service
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public string _verificationId { get; set; }
        //private TaskCompletionSource<bool> _phoneAuthTcs;
        TaskCompletionSource<Dictionary<bool, string>> keyValuePairs;


        public FirebaseAuthenticator()
        {
        }

        public Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var tcs = new TaskCompletionSource<string>();
                Auth.DefaultInstance.SignInWithPasswordAsync(username, password)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
                return tcs.Task;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-LoginAsync", ex.Message, "Ok");
                throw;
            }
        }

        public Task<Dictionary<bool, string>> SendOtpCodeAsync(string phoneNumber)
        {
            try
            {
                phoneNumber = (string)App.Current.Resources["CountryCode"] + phoneNumber;
                //_phoneAuthTcs = new TaskCompletionSource<bool>();

                PhoneAuthProvider.DefaultInstance.VerifyPhoneNumber(
                    phoneNumber,
                    null,
                    new VerificationResultHandler(OnVerificationResult));

                keyValuePairs = new TaskCompletionSource<Dictionary<bool, string>>();

                return keyValuePairs.Task;
                //return _phoneAuthTcs.Task;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-SendOtpCodeAsync", ex.Message, "Ok");
                throw;
            }
        }

        private void OnVerificationResult(string verificationId, NSError error)
        {
            try
            {
                Dictionary<bool, string> keyValues = new Dictionary<bool, string>();

                if (error != null)
                {
                    // something went wrong
                    keyValues.Add(false, Constraints.CouldNotSentOTP);
                }
                else
                {
                    _verificationId = verificationId;
                    keyValues.Add(true, Constraints.OTPSent);
                }
                keyValuePairs?.TrySetResult(keyValues);
                //keyValuePairs?.TrySetResult(true);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-OnVerificationResult", ex.Message, "Ok");
                throw;
            }
        }

        public Task<string> VerifyOtpCodeAsync(string code)
        {
            try
            {
                var tcs = new TaskCompletionSource<string>();

                var credential = PhoneAuthProvider.DefaultInstance.GetCredential(
                    _verificationId, code);
                Auth.DefaultInstance.SignInWithCredentialAsync(credential)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));

                return tcs.Task;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-VerifyOtpCodeAsync", ex.Message, "Ok");
                throw;
            }
        }

        private async void OnAuthCompleted(Task<AuthDataResult> task, TaskCompletionSource<string> tcs)
        {
            try
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    // something went wrong
                    tcs.SetResult(string.Empty);
                    return;
                }
                // user is logged in
                var result = task.Result;
                var token = await result.User.GetIdTokenAsync(false);
                tcs.SetResult(token);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-OnAuthCompleted", ex.Message, "Ok");
                throw;
            }
        }

        public Task<AuthenticatedUser> GetUserAsync()
        {
            try
            {
                var tcs = new TaskCompletionSource<AuthenticatedUser>();

                Firebase.CloudFirestore.Firestore.SharedInstance
                    .GetCollection("users")
                    .GetDocument(Auth.DefaultInstance.CurrentUser.Uid)
                    .GetDocument((snapshot, error) =>
                    {
                        if (error != null)
                        {
                            // something went wrong
                            tcs.TrySetResult(default(AuthenticatedUser));
                            return;
                        }
                        tcs.TrySetResult(new AuthenticatedUser
                        {
                            Id = snapshot.Id,
                            FirstName = snapshot.GetValue(new NSString("FirstName")).ToString(),
                            LastName = snapshot.GetValue(new NSString("LastName")).ToString()
                        });
                    });

                return tcs.Task;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-GetUserAsync", ex.Message, "Ok");
                throw;
            }
        }

        public Task<bool> Signout()
        {
            try
            {
                var tcs = new TaskCompletionSource<bool>();
                try
                {
                    Auth.DefaultInstance.SignOut(out NSError error);
                    tcs.TrySetResult(true);
                    return tcs.Task;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex.InnerException.InnerException);
                }
                tcs.TrySetResult(false);
                return tcs.Task;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-Signout", ex.Message, "Ok");
                throw;
            }
        }
    }
}