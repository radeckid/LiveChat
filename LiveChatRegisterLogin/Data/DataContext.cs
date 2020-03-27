using LiveChatRegisterLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveChatRegisterLogin.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Password> Passwords { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(t => t.Sender)
                .WithMany(m => m.SentMessages)
                .HasForeignKey(p => p.SenderId);

            modelBuilder.Entity<Message>()
                .HasOne(t => t.Receiver)
                .WithMany(m => m.ReceivedMessages)
                .HasForeignKey(p => p.ReceiverId);

            modelBuilder.Entity<Friend>()
                .HasOne(t => t.FirstUser)
                .WithMany(m => m.FirstFriends)
                .HasForeignKey(p => p.FirstUserId);

            modelBuilder.Entity<Friend>()
                .HasOne(t => t.SecondUser)
                .WithMany(m => m.SecondFriends)
                .HasForeignKey(p => p.SecondUserId);
        }

    }
}