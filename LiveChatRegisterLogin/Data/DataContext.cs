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
        public DbSet<Relation> Friends { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMembership> ChatMemberships { get; set; }
        public DbSet<UserTrace> UsersTrace { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>()
                .HasOne(t => t.Sender)
                .WithMany(m => m.NotificationsSent)
                .HasForeignKey(p => p.SenderId);

            modelBuilder.Entity<Notification>()
                .HasOne(t => t.Receiver)
                .WithMany(m => m.NotificationsReceived)
                .HasForeignKey(p => p.ReceiverId);
        }

    }
}