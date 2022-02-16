using Android.App;
using aptdealzMExecutiveMobile.Droid.DependencService;
using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseAppOnBackButton))]

namespace aptdealzMExecutiveMobile.Droid.DependencService
{
    public class CloseAppOnBackButton : ICloseAppOnBackButton, IDisposable
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public void CloseApp()
        {
            try
            {
                var activity = (Activity)Xamarin.Forms.Forms.Context;
                activity.SetResult(Result.Canceled);
                activity.FinishAffinity();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CloseAppOnBackButton: " + ex.Message);
            }
        }

        public void Dispose()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}