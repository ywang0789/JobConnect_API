using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobConnect_API.Migrations
{
    /// <inheritdoc />
    public partial class fixseeduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Applications",
                keyColumn: "application_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0be43a5d-a77c-4a1a-8215-26d48b99763b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4ffe2f21-5900-4669-bcc9-7d84a93a31c9");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "first_name", "last_name", "role" },
                values: new object[,]
                {
                    { "0be43a5d-a77c-4a1a-8215-26d48b99763b", 0, "c4532072-284d-414a-a218-3817a7ef5586", "recruiter@example.com", true, false, null, "RECRUITER@EXAMPLE.COM", "RECRUITER@EXAMPLE.COM", "AQAAAAIAAYagAAAAELmWxtf3vOJYCcOAtNYdECD3liZTCQ+wt/9850XrW6ygAy+CXwKagw7YRZxZZxQeNA==", null, false, "11d8e0b2-c378-404d-8e29-5e1e04dd8e73", false, "recruiter@example.com", "John", "Doe", "recruiter" },
                    { "4ffe2f21-5900-4669-bcc9-7d84a93a31c9", 0, "0d8ade6f-2622-4a38-b318-adcf37dbd6ad", "applicant@example.com", true, false, null, "APPLICANT@EXAMPLE.COM", "APPLICANT@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJ0IRI37Mq1Y26GcXMNx0Zt1Co3CenOIFEMnFjqcplRZnkI+O+escryYF4dKAnm4JA==", null, false, "a8156ca7-adc0-4865-a8dd-e8ca65954606", false, "applicant@example.com", "Jane", "Smith", "applicant" }
                });

            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "application_id", "account_id", "content", "job_id", "status" },
                values: new object[] { 1, "4ffe2f21-5900-4669-bcc9-7d84a93a31c9", "Hire me plz :pray:", 1, "Pending" });
        }
    }
}
