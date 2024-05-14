using Microsoft.AspNetCore.Mvc;
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

        public async Task<Picture?> CreatePicture(IFormFile file)
        {
            try
            {
                Image image = Image.Load(file.OpenReadStream());

                Picture picture = new Picture()
                {
                    Id = 0,
                    FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                    MimeType = file.ContentType
                };

                image.Save(Directory.GetCurrentDirectory() + "/images/lg/" + picture.FileName);
                image.Mutate(i => i.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Min,
                    Size = new Size() { Width = 320 }
                }));
                image.Save(Directory.GetCurrentDirectory() + "/images/sm/" + picture.FileName);

                _context.Pictures.Add(picture);
                await _context.SaveChangesAsync();
                return picture;
            }
            catch (Exception) { throw; }
        }

        public async Task<Picture?> GetPicture(int id)
        {
            if (IsContextNull()) return null;
            return await _context.Pictures.FindAsync(id);
        }

        public async Task<IActionResult> deletePicture(int id)
        {
            Picture? picture = await _context.Pictures.FindAsync(id);
            if (picture == null) return new NotFoundResult();

            if(picture.FileName != null && picture.MimeType != null)
            {
                System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/lg/" + picture.FileName);
                System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/sm/" + picture.FileName);
            }
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }



}
