using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<TakenBook> TakenBooks { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-6A8L3F5;Database=LibraryAppDB;Trusted_Connection=True;TrustServerCertificate=True");
    }
}