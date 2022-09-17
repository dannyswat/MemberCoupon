using MemberCoupon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MemberCoupon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<Redemption> Redemptions { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<MemberGroup> MemberGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Member>().HasIndex(e => e.Number).IsUnique();

            builder.Entity<Redemption>()
                .HasOne(e => e.Member)
                .WithMany(e => e.Redemptions)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Redemption>()
                .HasOne(e => e.Coupon)
                .WithMany(e => e.Redemptions)
                .HasForeignKey(e => e.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Member>()
                .HasOne(e => e.MemberGroup)
                .WithMany()
                .HasForeignKey(e => e.MemberGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Coupon>()
                .HasOne(e => e.ExclusiveMemberGroup)
                .WithMany()
                .HasForeignKey(e => e.ExclusiveMemberGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}