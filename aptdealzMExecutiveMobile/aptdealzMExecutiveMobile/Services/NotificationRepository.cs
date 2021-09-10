﻿using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using System;
using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Services
{
    public class NotificationRepository : INotificationRepository
    {
        NotificationAPI notificationAPI = new NotificationAPI();

        public async Task<string> GetNotificationCount()
        {
            string NotificationCount = string.Empty;
            try
            {
                var mResponse = await notificationAPI.GetNotificationsCountForUser();
                if (mResponse != null && mResponse.Succeeded)
                {
                    int notificationCount = 0;
                    int.TryParse(mResponse.Data.ToString(), out notificationCount);
                    if (notificationCount > 0)
                    {
                        NotificationCount = notificationCount.ToString();
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NavigationRepository/GetNotificationsCount: " + ex.Message);
            }
            return NotificationCount;
        }

        public async Task<bool> SetUserNoficiationAsRead(string NotificationId)
        {
            bool hasReaded = false;
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await notificationAPI.SetUserNoficiationAsRead(NotificationId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    hasReaded = (bool)mResponse.Data;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NavigationRepository/GetNotificationsCount: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return hasReaded;
        }
    }
}
