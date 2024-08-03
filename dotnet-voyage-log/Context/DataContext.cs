using dotnet_voyage_log.Models;
using dotnet_voyage_log.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Context;
public class DataContext : DbContext
{
    protected readonly Configs _config;

    public DataContext(Configs configuration)
    {
        _config = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        Console.WriteLine(JsonConvert.SerializeObject(_config, Formatting.Indented));
        // connect to postgres with connection string from app settings
        options
            .UseNpgsql(_config.GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Id).UseIdentityAlwaysColumn();
        modelBuilder
            .Entity<User>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");         
        modelBuilder
            .Entity<Voyage>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");
         modelBuilder
            .Entity<Voyage>()
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()");
        modelBuilder.Entity<Voyage>().Property(b => b.Id).UseIdentityAlwaysColumn();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Voyage> Voyages { get;set; }
}