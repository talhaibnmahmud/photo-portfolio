using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoTest.Areas.Identity.Data;
using PhotoTest.Models;

namespace PhotoTest.Data;

public class PhotoTestContext : IdentityDbContext<PhotoTestUser>
{
    public PhotoTestContext(DbContextOptions<PhotoTestContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Post>? Post { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Favorite>? Favorites { get; set; }
}
