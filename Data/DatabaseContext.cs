using chat_be.Models;
using chat_be.Models.Responses;
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

        public DbSet<MakeFriendModel> MakeFriendModels { get; set; }

        public DbSet<MessageModel> MessageModels { get; set; }

        public DbSet<MessageGroupUserModel> MessageGroupUserModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MakeFriendModel>()
                .HasKey(x => new { x.UserId, x.FriendId });
            modelBuilder.Entity<UserModel>()
            .HasData(
                new UserModel("admin", BCrypt.Net.BCrypt.HashPassword("admin"), UserRole.admin, "Admin") { Id = 1 },
                new UserModel("user1", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 1") { Id = 2 },
                new UserModel("user2", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 2") { Id = 3 },
                new UserModel("user3", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 3") { Id = 4 },
                new UserModel("user4", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 4") { Id = 5 },
                new UserModel("user5", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 5") { Id = 6 },
                new UserModel("user6", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 6") { Id = 7 },
                new UserModel("user7", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 7") { Id = 8 },
                new UserModel("user8", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 8") { Id = 9 },
                new UserModel("user9", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 9") { Id = 10 },
                new UserModel("user10", BCrypt.Net.BCrypt.HashPassword("user"), UserRole.user, "User 10") { Id = 11 }
                );
                modelBuilder.Entity<MessageGroupUserModel>()
                .HasOne(mgu => mgu.MessageGroup)
                .WithMany(mg => mg.MessageGroupUsers)
                .HasForeignKey(mgu => mgu.MessageGroupId);
                modelBuilder.Entity<MessageGroupUserModel>()
                .HasOne(mgu => mgu.User)
                .WithMany()
                .HasForeignKey(mgu => mgu.UserId);
        }

    }

    public static class IQueryableExtensions
    {
        public static async Task<PaginatedResponse<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source, int pageNumber, int pageSize
        )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalPage = (int)Math.Ceiling(count / (double)pageSize);
            return new PaginatedResponse<T>(totalPage, pageNumber, pageSize, count, items);
        }

    }
}

