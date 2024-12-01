using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BMS_API.Models
{
    public class tblGallery
    {
        public long GalleryID { get; set; }
        public long ActivityIDF { get; set; }
        public long UserIDF { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

    }

    public class tblGalleryImageResponse
    {
        public long UserIDF { get; set; }
        public long GalleryID { get; set; }
        public string? ImageUrl { get; set; }

    }


    public class tblGalleryRequestModel
    {
        public long GalleryID { get; set; }
        public long ActivityIDF { get; set; }
        public IList<IFormFile> ImageUrl { get; set; }

    }
}
