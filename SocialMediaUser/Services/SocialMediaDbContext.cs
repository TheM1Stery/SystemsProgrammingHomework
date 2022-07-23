using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMediaUser.Models;

namespace SocialMediaUser.Services;

public class SocialMediaDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json").Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection") ?? string.Empty);
        base.OnConfiguring(optionsBuilder);
    }
    
    public DbSet<User> Users => Set<User>();
}