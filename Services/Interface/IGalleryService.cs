using BMS_API.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace BMS_API.Services.Interface
{
    public interface IGalleryService : ICommon
    {
        Task<Int64> Gallery_Insert(tblGallery modelActivity);
        Task<Int64> Gallery_Update(tblGallery modelActivity);
        Task<tblGallery> GetGalleryImageById(long GalleryID);
        Task<List<tblGalleryImageResponse>> GetGalleryImagesByUserAsync(long UserIDF);
    }
}
