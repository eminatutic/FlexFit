using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexFit.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPaidToPenaltyCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "PenaltyCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "PenaltyCards");
        }
    }
}
