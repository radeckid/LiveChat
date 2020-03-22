using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public class DataContext : DbContext
    {
        private static DataContext _instance = null;
        private readonly static object padlock = new object();

        private DataContext() { }

        public static DataContext GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataContext();
                    }
                    return _instance;
                }
            }

            return _instance;
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //if(!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseMySql("server=localhost;database=livechat;user=root;password=root");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });
        }


    }
}