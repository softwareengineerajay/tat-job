using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOV.ES.TAT.Job.API.Migrations
{
    public partial class AllTableCreationJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "job");

            migrationBuilder.CreateTable(
                name: "ClientRequest",
                schema: "job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NovJob",
                schema: "job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobNumber = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    ModuleKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BusinessUnit = table.Column<int>(type: "int", nullable: true),
                    CorpRigId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Customer = table.Column<int>(type: "int", nullable: true),
                    CorpWellSiteId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedSource = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ModifiedSource = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TATReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobSnapShot",
                schema: "job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    PreviousData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedSource = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ModifiedSource = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TATReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSnapShot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSnapShot_NovJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "job",
                        principalTable: "NovJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSnapShot_JobId",
                schema: "job",
                table: "JobSnapShot",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRequest",
                schema: "job");

            migrationBuilder.DropTable(
                name: "JobSnapShot",
                schema: "job");

            migrationBuilder.DropTable(
                name: "NovJob",
                schema: "job");
        }
    }
}
