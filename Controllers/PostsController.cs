using BMS_API.Models.User;
using BMS_API.Services.Interface;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BudgetManagement.Controllers;
using System.Linq;
using BMS_API.Models.Partner;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Http.Headers;
using BMS_API.Models;

namespace BMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : CommonController
    {
       
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IPostsService _PostsService;

        public PostsController(BMSContext context,IPostsService postsService, IDashboardService __DashboardService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _PostsService = postsService;
        }


        #region INSERT-UPDATE
        [HttpPost]
        [Route("Posts-insert-update")]
        public async Task<IActionResult> Posts_InsertUpdate([FromForm] tblPostsRequestModel modelActivity)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _PostsService.ObjUser = objUser;

                var missingFields = new List<string>();

                if (modelActivity.ActivityIDF == null) missingFields.Add("ActivityIDF");
                if (string.IsNullOrWhiteSpace(modelActivity.PostDescription)) missingFields.Add("PostDescription");
                if (modelActivity.PostImage == null) missingFields.Add("PostImage");

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

                foreach (IFormFile source in modelActivity.PostImage)
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

                tblPosts postRequestModel = new tblPosts();
                postRequestModel.PostID = modelActivity.PostID;
                postRequestModel.ActivityIDF = modelActivity.ActivityIDF;
                postRequestModel.UserIDF = objUser.UserID;
                postRequestModel.PostDescription = modelActivity.PostDescription;
                postRequestModel.CreatedDate = DateTime.Now;
                postRequestModel.PostImage = response.FileName;
               

                if (modelActivity.PostID > 0)
                {
                    paramIdentity = await _PostsService.Posts_Update(postRequestModel);
                    if (paramIdentity == 0)
                    {
                        paramIdentityAction = msgUpdated;

                        return Ok(new { status = 200, data = new object[] { }, message = " Posts Updated Sucessfully." });
                    }

                }
                else
                {
                    paramIdentity = await _PostsService.Posts_Insert(postRequestModel);
                    if (paramIdentity == 0)
                    {
                        return BadRequest(new { status = 201, data = new object[] { }, message = "Posts not created." });
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




                var Posts = await _PostsService.GetPostsById(paramIdentity);

                return Ok(new { status = 200, data = Posts, message = "Post created successfully." });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPosts_Insert_Update", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }


        private string GetFullPathOfFile(string fileName)
        {
            return $"{this.webHostEnvironment.WebRootPath}\\Uploads\\{fileName}";
        }
        #endregion INSERT-UPDATE


        #region Get-Posts
        [Route("Get-Posts")]
        [HttpPost]
        public async Task<IActionResult> GetPosts()
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _PostsService.ObjUser = objUser;

            try
            {
                dynamic response = await _PostsService.GetPostsAsync(objUser.UserID);
                if (response.data == null || !((IEnumerable<object>)response.data).Any())
                {
                    return Ok(new { status = 201, data = "No data found" });
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                await _PostsService.ErrorLog(201, e.Message, $"Controller : Posts_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion

          
    }
}
