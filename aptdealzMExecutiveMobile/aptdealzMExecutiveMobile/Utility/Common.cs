using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace aptdealzMExecutiveMobile.Utility
{
    public class Common
    {
        #region DisplayMessages
        public static void DisplayErrorMessage(string errormessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(errormessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = Color.FromHex("#f8d7da"),
                MessageTextColor = Color.FromHex("#721c24"),
                Duration = new TimeSpan(0, 0, 5),
            });
        }

        public static void DisplaySuccessMessage(string successmessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(successmessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = Color.FromHex("#d4edda"),
                MessageTextColor = Color.FromHex("#155724"),
                Duration = new TimeSpan(0, 0, 5)
            });
        }

        public static void DisplayWarningMessage(string warningmessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(warningmessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = Color.FromHex("#fff3cd"),
                MessageTextColor = Color.FromHex("#856404"),
                Duration = new TimeSpan(0, 0, 5),
            });
        }
        #endregion

        #region Animation
        public static async void BindAnimation(ImageButton imageButton = null, Button button = null, Grid grid = null, StackLayout stackLayout = null, Label label = null,Image image = null)
        {
            if (imageButton != null)
            {
                await imageButton.ScaleTo(0.9, 100, Easing.Linear);
                await imageButton.ScaleTo(1, 100, Easing.Linear);
            }

            if (button != null)
            {
                await button.ScaleTo(0.9, 100, Easing.Linear);
                await button.ScaleTo(1, 100, Easing.Linear);
            }

            if (grid != null)
            {
                await grid.ScaleTo(0.9, 100, Easing.Linear);
                await grid.ScaleTo(1, 100, Easing.Linear);
            }

            if (stackLayout != null)
            {
                await stackLayout.ScaleTo(0.9, 100, Easing.Linear);
                await stackLayout.ScaleTo(1, 100, Easing.Linear);
            }

            if (label != null)
            {
                await label.ScaleTo(0.9, 100, Easing.Linear);
                await label.ScaleTo(1, 100, Easing.Linear);
            }
            
            if (image != null)
            {
                await image.ScaleTo(0.9, 100, Easing.Linear);
                await image.ScaleTo(1, 100, Easing.Linear);
            }
        }
        #endregion
    }
}
