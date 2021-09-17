using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApiTest.Models;

namespace WebApiTest.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            
        }

        public DbSet<TodoItem> TodoItems { get; set; }

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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().Property(x => x.CreatedDate).HasColumnType("date");
        }
    }
}
