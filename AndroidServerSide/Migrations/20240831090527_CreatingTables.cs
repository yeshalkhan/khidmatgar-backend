using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndroidServerSide.Migrations
{
    /// <inheritdoc />
    public partial class CreatingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "ServiceProviders",
                newName: "ServiceName");

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "ServiceProviders",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceName",
                table: "ServiceProviders",
                newName: "Category");

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "ServiceProviders",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
