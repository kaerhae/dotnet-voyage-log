using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using dotnet_voyage_log.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using BC = BCrypt.Net.BCrypt;

namespace dotnet_voyage_log.Context;
public class DataContext : DbContext
{
    protected readonly IConfigs _config;

    public DataContext(IConfigs configuration)
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
        modelBuilder.Entity<User>()
            .Property(b => b.Id)
            .UseIdentityAlwaysColumn();
        modelBuilder
            .Entity<User>()
            .HasData(new User(){
                Id = 1,
                Username = _config.GetAdminUsername(),
                Email = _config.GetAdminEmail(),
                PasswdHash = BC.HashPassword(_config.GetAdminPassword()),
                AppRole = "admin"
            });
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
            .Property(b => b.Id)
            .UseIdentityAlwaysColumn();

        modelBuilder
            .Entity<Country>()
            .Property(b => b.Id)
            .UseIdentityAlwaysColumn();

        modelBuilder
            .Entity<Region>()
            .Property(b => b.Id)
            .UseIdentityAlwaysColumn();

        modelBuilder
            .Entity<Country>()
            .HasMany(x => x.Regions)
            .WithOne(x => x.Country)
            .HasForeignKey(x => x.CountryId);
        modelBuilder
            .Entity<Region>()
            .HasOne(x => x.Country)
            .WithMany(x => x.Regions)
            .HasForeignKey(x => x.CountryId);

        modelBuilder
            .Entity<Country>()
            .HasMany(x => x.Voyages)
            .WithOne(x => x.Country)
            .HasForeignKey(x => x.CountryId);
        modelBuilder
            .Entity<Region>()
            .HasMany(x => x.Voyages)
            .WithOne(x => x.Region)
            .HasForeignKey(x => x.RegionId);

        modelBuilder
            .Entity<Voyage>()
            .HasOne(x => x.Country)
            .WithMany(x => x.Voyages)
            .HasForeignKey(x => x.CountryId);

        modelBuilder
            .Entity<Voyage>()
            .HasOne(x => x.Region)
            .WithMany(x => x.Voyages)
            .HasForeignKey(x => x.RegionId);

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Voyage> Voyages { get;set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get;set; }
}