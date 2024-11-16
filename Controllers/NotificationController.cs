using HomoeopathyWorld.Models;
using HomoeopathyWorld.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using HomoeopathyWorld.Models.Utility;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace HomoeopathyWorld.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController
    {
        private readonly INotificationService _NotificationService;
        private readonly IWebHostEnvironment webHostEnvironment;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public NotificationController(INotificationService NotificationService, IWebHostEnvironment webHostEnvironment)
        {
            _NotificationService = NotificationService;
            this.webHostEnvironment = webHostEnvironment;
        }
        

        [HttpPost]
        [Route("Notification-check")]
        public async Task<IActionResult> Notification_Check()
        {
            try
            {
            }
            catch
            {
                
            }
            return null;
        }

        [HttpPost]
        [Route("Notification-get-all")]
        public async Task<IActionResult> Notification_GetAll()
        {
            try
            {
            }
            catch
            {
            }
                return null;
        }
    }
}
