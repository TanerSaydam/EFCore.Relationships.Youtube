using EFCore.Relationships.Youtube.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Relationships.Youtube.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserInformation> UsersInformations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
       .HasOne(p => p.UserInformation)
       .WithOne()
       .HasForeignKey<User>(p => p.UserInformationId)
       .OnDelete(DeleteBehavior.NoAction);
    }
}
