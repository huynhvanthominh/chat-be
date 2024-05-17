using chat_be.Models;
using Microsoft.EntityFrameworkCore;

namespace chat_be.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<MessageGroupModel> MessageGroupModels { get; set; }

    }
}