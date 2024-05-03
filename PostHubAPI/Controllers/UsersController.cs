using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using PostHubAPI.Models;
using PostHubAPI.Models.DTOs;
using PostHubAPI.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace PostHubAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly UserManager<User> _userManager;
        readonly PictureService _pictureService;

        public UsersController(UserManager<User> userManager, PictureService pictureService)
        {
            _userManager = userManager;
            _pictureService = pictureService;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO register)
        {
            if (register.Password != register.PasswordConfirm)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Message = "Les deux mots de passe spécifiés sont différents." });
            }
            User user = new User()
            {
                UserName = register.Username,
                Email = register.Email
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, register.Password);
            if (!identityResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "La création de l'utilisateur a échoué." });
            }
            return Ok(new { Message = "Inscription réussie ! 🥳" });
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            User user = await _userManager.FindByNameAsync(login.Username) ?? await _userManager.FindByEmailAsync(login.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                List<Claim> authClaims = new List<Claim>();
                foreach (string role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes("LooOOongue Phrase SiNoN Ça ne Marchera PaAaAAAaAas !"));
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: "https://localhost:7007",
                    audience: "http://localhost:4200",
                    claims: authClaims,
                    expires: DateTime.Now.AddMinutes(300),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    validTo = token.ValidTo,
                    username = user.UserName // Ceci sert déjà à afficher / cacher certains boutons côté Angular
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Message = "Le nom d'utilisateur ou le mot de passe est invalide." });
            }
        }

        [HttpPut]
        public async Task<ActionResult> ChangePassword()
        {
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null) return Unauthorized();

            string? oldPassword = Request.Form["oldPassword"];
            string? newPassword = Request.Form["newPassword"]; 
            string? newPasswordConfirm = Request.Form["newPasswordConfirm"];
            // Vérification du nouveau mot de passe
            if (newPassword != newPasswordConfirm)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Message = "Les deux mots de passe spécifiés sont différents." });
            }
            IdentityResult identityResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!identityResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "La modification du mot de passe a échoué." });
            }
            return Ok(new { Message = "Le mot de passe à été changé!" });
        }


        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetUserAvatar(string username)
        {
            User user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Message = "Le nom d'utilisateur est invalide." });
            }
            else
            {
                if (user.FileName == null || user.MimeType == null)
                {
                    byte[] bytesDefault = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/sm/default.png");
                    return File(bytesDefault, "image/png");
                }

                byte[] bytes = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/sm/" + user.FileName);
                return File(bytes, user.MimeType);
            }
        }

        [HttpPut]
        [Authorize]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> ChangeAvatar()
        {
            // Récupérer l'utilisateur
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null) return Unauthorized();

            // Récupérer le fichier
            IFormCollection formCollection = await Request.ReadFormAsync();
            IFormFile? file = formCollection.Files.GetFile("avatarImage");
            if (file != null)
            {
                Picture? newPicture = await _pictureService.CreatePicture(file);
                if (newPicture == null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                // Supprimer l'ancienne image
                if (user.FileName != null && user.MimeType != null)
                {
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/lg/" + user.FileName);
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/sm/" + user.FileName);
                }

                // Mettre à jour l'utilisateur
                user.FileName = newPicture.FileName;
                user.MimeType = newPicture.MimeType;
                await _userManager.UpdateAsync(user);
            }

            return Ok();
        }

        [HttpPut("{username}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MakeUserModerator(string username)
        {
            User? newModerator = await _userManager.FindByNameAsync(username);
            if (newModerator == null)
            {
                return NotFound(new { Message = "Cet utilisateur n'existe pas." });
            }
            await _userManager.AddToRoleAsync(newModerator, "moderator");
            return Ok(new {Message = username + " est maintenant un moderator!"});
        }
    }
}
