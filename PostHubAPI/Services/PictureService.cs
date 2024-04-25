using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace PostHubAPI.Services
{
    public class PictureService
    {
        private readonly PostHubAPIContext _context;

        public PictureService(PostHubAPIContext context) 
        {
            _context = context;
        }

        private bool IsContextNull() => _context.Pictures == null;

        public async Task<Picture?> CreatePicture(string fileName, string mimeType)
        {
            if (IsContextNull()) return null;
            Picture newPicture = new Picture() 
            {
                Id = 0,
                FileName = fileName,
                MimeType = mimeType
            };

            _context.Pictures.Add(newPicture);
            await _context.SaveChangesAsync();

            return newPicture;
        }

        public async Task<Picture?> GetPicture(int id)
        {
            if (IsContextNull()) return null;
            return await _context.Pictures.FindAsync(id);
        }
    }
}
