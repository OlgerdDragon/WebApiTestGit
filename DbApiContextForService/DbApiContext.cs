using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DbApiContextForService.Models;

namespace DbApiContextForService
{
    public class DbApiContext : DbContext
    {
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Husband> Husbands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<WantedProduct> WantedProducts { get; set; }
        public virtual DbSet<Wife> Wifes { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        public DbApiContext(DbContextOptions<DbApiContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Product>()
                .HasOne<Models.Shop>(p => p.Shops)
                .WithMany(b => b.Products)
                .HasForeignKey(o => o.ShopId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Models.WantedProduct>()
                .HasOne<Models.Product>(p => p.Products)
                .WithMany(b => b.WantedProducts)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Models.WantedProduct>()
                .HasOne<Models.Wife>(p => p.Wifes)
                .WithMany(b => b.WantedProducts)
                .HasForeignKey(o => o.WifeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();


            modelBuilder.Entity<Models.Wife>()
                .HasOne<Models.Husband>(p => p.Husbands)
                .WithOne(b => b.Wifes)
                .HasForeignKey<Models.Husband>(o => o.WifeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
                

            modelBuilder.Entity<Models.Person>()
                .HasOne<Models.Admin>(p => p.Admins)
                .WithOne(b => b.Persons)
                .HasForeignKey<Models.Admin>(o => o.PersonId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Models.Person>()
               .HasOne<Models.Husband>(p => p.Husbands)
               .WithOne(b => b.Persons)
               .HasForeignKey<Models.Husband>(o => o.PersonId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();
            modelBuilder.Entity<Models.Person>()
               .HasOne<Models.Wife>(p => p.Wifes)
               .WithOne(b => b.Persons)
               .HasForeignKey<Models.Wife>(o => o.PersonId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .ConfigureWarnings(b => b.Ignore(CoreEventId.StartedTracking))
                .ConfigureWarnings(b => b.Ignore(CoreEventId.QueryExecutionPlanned))
                .ConfigureWarnings(b => b.Ignore(CoreEventId.ContextInitialized))
                .ConfigureWarnings(b => b.Ignore(CoreEventId.QueryCompilationStarting))
                .LogTo(Console.WriteLine);
    }

    
}
