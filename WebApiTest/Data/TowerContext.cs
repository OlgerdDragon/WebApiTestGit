using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApiTest.Models;

namespace WebApiTest.Data
{
    public class TowerContext : DbContext
    {
        public TowerContext(DbContextOptions<TowerContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Product>()
                .HasOne<Models.Shop>(p => p.Shop)
                .WithMany(b => b.Products)
                .HasForeignKey(o => o.ShopId)
                .IsRequired();
            modelBuilder.Entity<Models.Husband>()
                .HasOne<Models.Wife>(p => p.Wife)
                .WithMany(b => b.Husbands)
                .HasForeignKey(o => o.WifeId)
                .IsRequired();
            modelBuilder.Entity<Models.Wife>()
                .HasOne<Models.WantedList>(p => p.WantedList)
                .WithMany(b => b.Wifes)
                .HasForeignKey(o => o.WantedListId)
                .IsRequired();

        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Husband> Husbands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<WantedList> WantedLists { get; set; }
        public DbSet<Wife> Wifes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .ConfigureWarnings(b => b.Ignore(CoreEventId.StartedTracking))
                .ConfigureWarnings(b => b.Ignore(CoreEventId.QueryExecutionPlanned))
                .ConfigureWarnings(b => b.Ignore(RelationalEventId.CommandExecuted))
                .ConfigureWarnings(b => b.Ignore(RelationalEventId.CommandCreating))
                .ConfigureWarnings(b => b.Ignore(RelationalEventId.CommandCreated))
                .ConfigureWarnings(b => b.Ignore(CoreEventId.ContextInitialized))
                .ConfigureWarnings(b => b.Ignore(CoreEventId.QueryCompilationStarting))
                .ConfigureWarnings(b => b.Ignore(RelationalEventId.ConnectionOpening))
                .LogTo(Console.WriteLine);
    }

    
}
