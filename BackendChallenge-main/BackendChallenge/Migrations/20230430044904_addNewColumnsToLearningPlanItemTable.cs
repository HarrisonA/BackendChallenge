using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendChallenge.Migrations
{
    public partial class addNewColumnsToLearningPlanItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "LearningPlanItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LearningItemName",
                table: "LearningPlanItems",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "LearningPlanItems");

            migrationBuilder.DropColumn(
                name: "LearningItemName",
                table: "LearningPlanItems");
        }
    }
}
