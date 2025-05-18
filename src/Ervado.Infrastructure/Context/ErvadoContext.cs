using Ervado.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ervado.Infrastructure.Context
{
    public class ErvadoContext : IdentityDbContext<IdentityUser>
    {
        public ErvadoContext(DbContextOptions<ErvadoContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            

            // bişey sorucam users diye bir tablo yok hatası veriyor ya senin burda yaptığın users tablosunu ıdentitiye mi bağlamak?

            // Identity tablolarının isimlerini özelleştirme
            builder.Entity<IdentityUser>().ToTable("ApplicationUsers");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}
