using HomoeopathyWorld.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HomoeopathyWorld.Models.Utility;

namespace HomoeopathyWorld.Services
{
    public interface INotificationService
    {
        Task<bool> Notification_Check();
        Task<string> Notification_GetAll(bool IsViewAll);
    }
}
