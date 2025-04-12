using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobConnect_API.Migrations
{
    /// <inheritdoc />
    public partial class newconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_account_id",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_job_id",
                table: "Applications");

            migrationBuilder.AlterColumn<int>(
                name: "job_id",
                table: "Applications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "account_id",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_account_id",
                table: "Applications",
                column: "account_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_job_id",
                table: "Applications",
                column: "job_id",
                principalTable: "Jobs",
                principalColumn: "job_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_account_id",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_job_id",
                table: "Applications");

            migrationBuilder.AlterColumn<int>(
                name: "job_id",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_id",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_account_id",
                table: "Applications",
                column: "account_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_job_id",
                table: "Applications",
                column: "job_id",
                principalTable: "Jobs",
                principalColumn: "job_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
