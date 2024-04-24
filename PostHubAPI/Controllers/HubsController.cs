using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Models;
using PostHubAPI.Models.DTOs;
using PostHubAPI.Services;

namespace PostHubAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HubsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly HubService _hubService;

        public HubsController(HubService hubService, UserManager<User> userManager)
        {
            _hubService = hubService;
            _userManager = userManager;
        }

        // GET: Obtenir la liste de hubs d'un utilisateur
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<HubDisplayDTO>>> GetUserHubs()
        {
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null) return Unauthorized();

            IEnumerable<Hub>? userHubs = _hubService.GetUserHubs(user);
            if (userHubs == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(userHubs.Select(h => new HubDisplayDTO()
            {
                Id = h.Id,
                Name = h.Name,
                IsJoined = h.Users?.Contains(user)
            }));
        }

        // POST: Créer un nouveau hub
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<HubDisplayDTO>> PostHub(Hub hub)
        {
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null) return Unauthorized();

            Hub? newHub = await _hubService.CreateHub(hub);
            if (newHub == null) return StatusCode(StatusCodes.Status500InternalServerError);

            Hub? joinedHub = await _hubService.ToggleJoinHub(newHub.Id, user);
            if(joinedHub == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(new HubDisplayDTO() { 
                Id = joinedHub.Id,
                Name = joinedHub.Name,
                IsJoined = joinedHub.Users?.Contains(user)
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HubDisplayDTO>> GetHub(int id)
        {
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Hub? hub = await _hubService.GetHub(id);
            if (hub == null) return NotFound();

            return Ok(new HubDisplayDTO()
            {
                Id = hub.Id,
                Name = hub.Name,
                IsJoined = user == null ? null : hub.Users?.Contains(user)
            });
        }

        [HttpPut("{hubId}")]
        [Authorize]
        public async Task<ActionResult> ToggleJoinHub(int hubId)
        {
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null) return BadRequest();

            Hub? hub = await _hubService.ToggleJoinHub(hubId, user);
            if (hub == null) return NotFound();

            return Ok(new { Message = hub.Users!.Contains(user) ? "Hub rejoint." : "Hub quitté."});
        }
    }
}
