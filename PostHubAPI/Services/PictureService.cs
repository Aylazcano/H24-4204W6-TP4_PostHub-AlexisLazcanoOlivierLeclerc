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
    }

}
