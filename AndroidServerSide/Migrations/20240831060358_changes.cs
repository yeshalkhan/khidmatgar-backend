using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndroidServerSide.Migrations
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceProviders",
                table: "ServiceProviders");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ServiceProviders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceProviders",
                table: "ServiceProviders",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceProviders",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ServiceProviders");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ServiceProviders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceProviders",
                table: "ServiceProviders",
                column: "Name");
        }
    }
}
