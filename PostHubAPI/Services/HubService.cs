using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Models;

namespace PostHubAPI.Services
{
    public class HubService
    {
        private readonly PostHubAPIContext _context;

        public HubService(PostHubAPIContext context)
        {
            _context = context;
        }

        public IEnumerable<Hub>? GetUserHubs(User user)
        {
            if (IsContextNull()) return null;

            if(user.Hubs == null) return new List<Hub>();

            return user.Hubs;
        }

        public async Task<Hub?> GetHub(int id)
        {
            if (IsContextNull()) return null;

            return await _context.Hubs.FindAsync(id);
        }

        public async Task<Hub?> ToggleJoinHub(int id, User user)
        {
            if (IsContextNull()) return null;

            Hub? hub = await _context.Hubs.FindAsync(id);
            if (hub == null) return null;

            hub.Users ??= new List<User>();

            if (hub.Users.Contains(user)) hub.Users.Remove(user);
            else hub.Users.Add(user);
            await _context.SaveChangesAsync();

            return hub;
        }

        public async Task<IEnumerable<Hub>?> GetAllHubs()
        {
            if (IsContextNull()) return null;

            return await _context.Hubs.ToListAsync();
        }

        public async Task<Hub?> CreateHub(Hub hub)
        {
            if (IsContextNull()) return null;

            _context.Hubs.Add(hub);
            await _context.SaveChangesAsync();
            return hub;
        }

        private bool IsContextNull() => _context.Hubs == null;
    }
}
