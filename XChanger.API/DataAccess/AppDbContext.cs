using Microsoft.EntityFrameworkCore;
using XChanger.API.Models.Data;

namespace XChanger.API.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Pet> Pets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().Property(p => p.Name).HasColumnType("TEXT");
        modelBuilder.Entity<Pet>().Property(p => p.Name).HasColumnType("TEXT");
    }
}