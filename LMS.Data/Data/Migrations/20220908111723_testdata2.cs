using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Web.Data.Migrations
{
    public partial class testdata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(975), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(973) });

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(978), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(977) });

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(982), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(980) });

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(985), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(984) });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(921), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(889) });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 2, "En annan kurs", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(925), "Kurs 2", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(924) });

            migrationBuilder.UpdateData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(951), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(949) });

            migrationBuilder.UpdateData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(955), new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(953) });

            migrationBuilder.InsertData(
                table: "Module",
                columns: new[] { "Id", "CourseId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 3, 2, "Tredje modulen", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(958), "Modul 3", new DateTime(2022, 9, 8, 13, 17, 23, 272, DateTimeKind.Local).AddTicks(957) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3755), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3753) });

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3759), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3758) });

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3762), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3761) });

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3766), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3764) });

            migrationBuilder.UpdateData(
                table: "Course",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3707), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3670) });

            migrationBuilder.UpdateData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3732), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3731) });

            migrationBuilder.UpdateData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3736), new DateTime(2022, 9, 8, 10, 24, 18, 500, DateTimeKind.Local).AddTicks(3735) });
        }
    }
}
