using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreImageUploadAssignment2.Migrations
{
    public partial class addcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Image",
                type: "nvarchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Image");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Employees",
                type: "varchar(200)",
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
    }
}
