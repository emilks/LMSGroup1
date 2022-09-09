using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Web.Data.Migrations
{
    public partial class testdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ActivityType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Module",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Activity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityTypeId",
                table: "Activity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Module",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActivityTypeId",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ActivityType",
                columns: new[] { "Id", "ActivityName" },
                values: new object[] { 1, "Föreläsning" });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 1, "En kurs", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(921), "Kurs 1", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(889) });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 2, "En annan kurs", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(925), "Kurs 2", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(924) });

            migrationBuilder.InsertData(
                table: "Module",
                columns: new[] { "Id", "CourseId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 1, 1, "Första modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(951), "Modul 1", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(949) });

            migrationBuilder.InsertData(
                table: "Module",
                columns: new[] { "Id", "CourseId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 2, 1, "Andra modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(955), "Modul 2", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(953) });

            migrationBuilder.InsertData(
                table: "Module",
                columns: new[] { "Id", "CourseId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 3, 2, "Tredje modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(958), "Modul 3", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(957) });

            migrationBuilder.InsertData(
                table: "Activity",
                columns: new[] { "Id", "ActivityTypeId", "Description", "EndDate", "ModuleId", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, "Första aktiviteten i första modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(975), 1, "Aktivitet 1", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(973) },
                    { 2, 1, "Andra aktiviteten i först modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(978), 1, "Aktivitet 2", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(977) },
                    { 3, 1, "Första aktiviteten i andra modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(982), 2, "Aktivitet 3", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(980) },
                    { 4, 1, "Andra aktiviteten i andra modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(985), 2, "Aktivitet 4", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(984) }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
