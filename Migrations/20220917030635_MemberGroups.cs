using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemberCoupon.Migrations
{
    public partial class MemberGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberGroupId",
                table: "Members",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExclusiveMemberGroupId",
                table: "Coupons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberGroupId",
                table: "Members",
                column: "MemberGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_ExclusiveMemberGroupId",
                table: "Coupons",
                column: "ExclusiveMemberGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_MemberGroups_ExclusiveMemberGroupId",
                table: "Coupons",
                column: "ExclusiveMemberGroupId",
                principalTable: "MemberGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberGroups_MemberGroupId",
                table: "Members",
                column: "MemberGroupId",
                principalTable: "MemberGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_MemberGroups_ExclusiveMemberGroupId",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberGroups_MemberGroupId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "MemberGroups");

            migrationBuilder.DropIndex(
                name: "IX_Members_MemberGroupId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_ExclusiveMemberGroupId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "MemberGroupId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ExclusiveMemberGroupId",
                table: "Coupons");
        }
    }
}
