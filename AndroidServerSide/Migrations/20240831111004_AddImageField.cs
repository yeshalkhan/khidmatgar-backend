using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndroidServerSide.Migrations
{
    /// <inheritdoc />
    public partial class AddImageField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ServiceProviders");
        }
    }
}
