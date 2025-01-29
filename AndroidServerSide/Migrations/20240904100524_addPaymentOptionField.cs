using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndroidServerSide.Migrations
{
    /// <inheritdoc />
    public partial class addPaymentOptionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentOption",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentOption",
                table: "Bookings");
        }
    }
}
