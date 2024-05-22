using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chat_be.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldNameInMessageGroupModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MessageGroupModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MessageGroupModels");
        }
    }
}
