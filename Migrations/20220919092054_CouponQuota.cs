using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemberCoupon.Migrations
{
    public partial class CouponQuota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quota",
                table: "Coupons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RedeemedCount",
                table: "Coupons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE Coupons SET RedeemedCount = (SELECT COUNT(*) FROM Redemptions WHERE Redemptions.CouponId = Coupons.Id)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quota",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "RedeemedCount",
                table: "Coupons");
        }
    }
}
