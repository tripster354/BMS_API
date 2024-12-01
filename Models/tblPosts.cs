using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BMS_API.Models
{
    public class tblPostsRequestModel
    {
        public long PostID { get; set; }
        public long ActivityIDF { get; set; }
        //public long UserIDF { get; set; }
        public string PostDescription { get; set; }
        public IList<IFormFile> PostImage { get; set; }
    }


    public class tblPosts
    {
        public long PostID { get; set; }
        public long ActivityIDF { get; set; }
        public long UserIDF { get; set; }
        public string PostDescription { get; set; }
        public int LikeStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string PostImage { get; set; }
        public string Comments { get; set; }
    }
}
