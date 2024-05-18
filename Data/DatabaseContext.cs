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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MakeFriendModel>()
                .HasKey(m => new { m.UserId, m.FriendId });
            modelBuilder.Entity<MakeFriendModel>()
                .HasOne(m => m.User)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MakeFriendModel>()
                .HasOne(m => m.Friend)
                .WithMany(u => u.MakeFriendRequests)
                .HasForeignKey(m => m.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
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
            // total page
            var totalPage = (int)Math.Ceiling(count / (double)pageSize);
            return new PaginatedResponse<T>(totalPage, pageNumber, pageSize, count, items);
        }

    }
}

