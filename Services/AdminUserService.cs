using chat_be.Data;
using chat_be.Models;
using chat_be.Services.Abstracts;

namespace chat_be.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<AdminUserService> _logger;

        public AdminUserService(
            DatabaseContext context,
            ILogger<AdminUserService> logger
        )
        {
            _context = context;
            _logger = logger;
        }
        public Task<List<UserModel>> GetUsers()
        {
            var users = _context.Users.ToList();
            return Task.FromResult(users);
        }
        public void initAdmin()
        {
            _logger.LogInformation("Checking for admin user");
            var admin = _context.Users.FirstOrDefault(u => u.Username == "admin");
            if (admin == null)
            {
                _context.Users.Add(new UserModel
                (
                 "admin",
                 "admin",
                  "admin"
                ));
                _context.SaveChanges();
            }
        }
    }
}