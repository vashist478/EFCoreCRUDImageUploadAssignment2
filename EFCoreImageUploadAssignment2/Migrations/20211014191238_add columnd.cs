using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreImageUploadAssignment2.Migrations
{
    public partial class addcolumnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ImageId",
                table: "Employees",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Image_ImageId",
                table: "Employees",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Image_ImageId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ImageId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Employees");
        }
    }
}
