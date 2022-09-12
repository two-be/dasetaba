using Microsoft.EntityFrameworkCore;

namespace Dasetaba.Data;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=host;Database=database;Username=username;Password=password");
    }
}