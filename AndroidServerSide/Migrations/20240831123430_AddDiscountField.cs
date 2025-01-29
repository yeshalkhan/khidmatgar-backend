using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndroidServerSide.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "ServiceProviders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ServiceProviders");
        }
    }
}
