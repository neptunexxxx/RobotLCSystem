using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDbContext.Migrations
{
    /// <inheritdoc />
    public partial class a1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lineCode",
                table: "real_time_product_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "materialCode",
                table: "real_time_product_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "passBeginTime",
                table: "real_time_product_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "passEndTime",
                table: "real_time_product_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "正则表达式",
                table: "material_bind_info",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "是否使用",
                table: "material_bind_info",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateTable(
                name: "exception_binding_information",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    snNumber = table.Column<string>(type: "text", nullable: false),
                    exceptionType = table.Column<string>(type: "text", nullable: false),
                    exceptionCode = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exception_binding_information", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exception_binding_information");

            migrationBuilder.DropColumn(
                name: "lineCode",
                table: "real_time_product_info");

            migrationBuilder.DropColumn(
                name: "materialCode",
                table: "real_time_product_info");

            migrationBuilder.DropColumn(
                name: "passBeginTime",
                table: "real_time_product_info");

            migrationBuilder.DropColumn(
                name: "passEndTime",
                table: "real_time_product_info");

            migrationBuilder.AlterColumn<string>(
                name: "正则表达式",
                table: "material_bind_info",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "是否使用",
                table: "material_bind_info",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
