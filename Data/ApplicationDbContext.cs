using Microsoft.EntityFrameworkCore;
using WebAPI_JWT.Models;

namespace WebAPI_JWT.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options){}

    public virtual DbSet<Product> Products{ get; set; }
    public virtual DbSet<UserInfo> UserInfos { get; set; }
}