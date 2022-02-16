﻿using Android;
using Android.Content;
using aptdealzMExecutiveMobile.Droid.DependencService;
using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Utility;
using Xamarin.Forms;

[assembly: Dependency(typeof(OpenWriteSettings))]
namespace aptdealzMExecutiveMobile.Droid.DependencService
{
    public class OpenWriteSettings : IOpenWriteSettings
    {
        public void GrantWriteSettings()
        {
            try
            {
                var activity = (MainActivity)Forms.Context;

                const string WriteSettingspermission = Manifest.Permission.WriteSettings;
                if (activity.CheckSelfPermission(WriteSettingspermission) != (int)Android.Content.PM.Permission.Granted)
                {
                    Intent intent = new Intent(Android.Provider.Settings.ActionManageWriteSettings);
                    intent.SetData(Android.Net.Uri.Parse("package:" + activity.PackageName));
                    activity.StartActivity(intent);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("OpenWriteSettings/GrantWriteSettings: " + ex.Message);
            }
        }
    }
}