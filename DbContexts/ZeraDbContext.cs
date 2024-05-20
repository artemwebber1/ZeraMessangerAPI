using Microsoft.EntityFrameworkCore;
using ZeraMessanger.Models;

namespace ZeraMessanger.DbContexts
{
    public class ZeraDbContext : DbContext
    {
        public ZeraDbContext()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Database.EnsureCreated();
        }

        #region Tables (DbSets)

        public DbSet<User> Users { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        #endregion

        private readonly IConfiguration _configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServerConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Chats)
                .WithMany(c => c.Members)
                .UsingEntity(e => e.ToTable("UserChats"));
        }
    }
}
