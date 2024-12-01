using BMS_API.Models;
using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using System.Linq;
using BMS_API.Services;

namespace BMS_API.Controllers
{
    public class GalleryController : CommonController
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IGalleryService _galleryService;

        public GalleryController(BMSContext context, IGalleryService galleryService, IDashboardService __DashboardService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _galleryService = galleryService;
        }

        #region INSERT-UPDATE
        [HttpPost]
        [Route("Gallery_InsertUpdate")]
        public async Task<IActionResult> Gallery_InsertUpdate([FromForm] tblGalleryRequestModel modelActivity)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _galleryService.ObjUser = objUser;

                var missingFields = new List<string>();

                if (modelActivity.ActivityIDF == null) missingFields.Add("ActivityIDF");;
                if (modelActivity.ImageUrl == null) missingFields.Add("ImageUrl");

                if (missingFields.Any())
                {
                    return BadRequest(new
                    {
                        status = 201,
                        data = new object[] { },
                        message = $"{string.Join(", ", missingFields)} field(s) missing, please input these fields."
                    });
                }

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";


                string fileName = null;

                foreach (IFormFile source in modelActivity.ImageUrl)
                {
                    // Get original file name to get the extension from it.
                    string orgFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName;

                    // Create a new file name to avoid existing files on the server with the same names.
                    // fileName = DateTime.Now.ToFileTime() + Path.GetExtension(orgFileName);
                    fileName = DateTime.Now.Second + orgFileName;


                    string fullPath = GetFullPathOfFile(fileName.Replace("\"", ""));

                    // Create the directory.
                    Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);

                    // Save the file to the server.
                    await using FileStream output = System.IO.File.Create(fullPath);
                    await source.CopyToAsync(output);
                }

                var response = new { FileName = fileName.Replace("\"", "") };
                //ViewBag.FileName = response.FileName;

                tblGallery galleryRequestModel = new tblGallery();
                galleryRequestModel.GalleryID = modelActivity.GalleryID;
                galleryRequestModel.ActivityIDF = modelActivity.ActivityIDF;
                galleryRequestModel.UserIDF = objUser.UserID;
                galleryRequestModel.ImageUrl = response.FileName;
                galleryRequestModel.CreatedDate = DateTime.Now;


                if (modelActivity.GalleryID > 0)
                {
                    paramIdentity = await _galleryService.Gallery_Update(galleryRequestModel);
                    if (paramIdentity == 0)
                    {
                        paramIdentityAction = msgUpdated;

                        return Ok(new { status = 200, data = new object[] { }, message = " Gallery Updated Sucessfully." });
                    }

                }
                else
                {
                    paramIdentity = await _galleryService.Gallery_Insert(galleryRequestModel);
                    if (paramIdentity == 0)
                    {
                        return BadRequest(new { status = 201, data = new object[] { }, message = "Gallery not created." });
                    }
                    else
                    {
                        paramIdentityAction = msgInserted; // Activity was successfully inserted
                    }
                }

                if (paramIdentity == -1)
                {
                    paramIdentityAction = msgDuplicate;

                    return BadRequest(new { status = 201, data = new object[] { }, message = "Duplicate Posts." });
                }




                var gallery = await _galleryService.GetGalleryImageById(paramIdentity);

                return Ok(new { status = 200, data = gallery, message = "GalleryImage Uploaded successfully." });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : GalleryController", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }


        private string GetFullPathOfFile(string fileName)
        {
            return $"{this.webHostEnvironment.WebRootPath}\\Uploads\\{fileName}";
        }
        #endregion INSERT-UPDATE


        #region Get All Gallery-Photos By User
        [Route("GetAllImagesByUser")]
        [HttpPost]
        public async Task<IActionResult> GetAllImagesByUser()
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _galleryService.ObjUser = objUser;

            try
            {
                // Call the updated service method that returns a list
                var response = await _galleryService.GetGalleryImagesByUserAsync(objUser.UserID);

                // Check if the response is empty
                if (response == null || !response.Any())
                {
                    return Ok(new { status = 201, data = "No data found" });
                }

                // Return the response
                return Ok(new { status = 200, data = response });
            }
            catch (Exception e)
            {
                // Log the error
                await _galleryService.ErrorLog(201, e.Message, "Controller: GalleryController", 1);

                // Return a bad request with error details
                return BadRequest(new { status = 0, message = "An error occurred", error = e.Message });
            }
        }
        #endregion


    }
}
