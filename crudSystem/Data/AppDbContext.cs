// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using CrudSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace CrudSystem.Data
{
public class AppDbContext :IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Employees> Employees { get; set; }
}
}