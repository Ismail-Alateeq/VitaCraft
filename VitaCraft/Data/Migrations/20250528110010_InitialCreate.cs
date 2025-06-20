using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaCraft.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "farstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PortFolios",
                columns: table => new
                {
                    portFolioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    personalImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateOnly>(type: "date", nullable: false),
                    modifiedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    thirdName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false),
                    linkedIn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gitHub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    link3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortFolios", x => x.portFolioId);
                    table.ForeignKey(
                        name: "FK_PortFolios_AspNetUsers_EndUserId",
                        column: x => x.EndUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    resumeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdDate = table.Column<DateOnly>(type: "date", nullable: false),
                    modifiedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    thirdName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false),
                    linkedIn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gitHub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    link3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.resumeId);
                    table.ForeignKey(
                        name: "FK_Resumes_AspNetUsers_EndUserId",
                        column: x => x.EndUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    serviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    serviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    portFolioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.serviceId);
                    table.ForeignKey(
                        name: "FK_Services_PortFolios_portFolioID",
                        column: x => x.portFolioID,
                        principalTable: "PortFolios",
                        principalColumn: "portFolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    certificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    providerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    topicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPA = table.Column<int>(type: "int", nullable: false),
                    resumeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.certificateId);
                    table.ForeignKey(
                        name: "FK_Certificates_Resumes_resumeID",
                        column: x => x.resumeID,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    educationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    degreeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    major = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPA = table.Column<int>(type: "int", nullable: false),
                    resumeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.educationId);
                    table.ForeignKey(
                        name: "FK_Educations_Resumes_resumeID",
                        column: x => x.resumeID,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    experienceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    isCurrent = table.Column<bool>(type: "bit", nullable: false),
                    duties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    resumeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.experienceId);
                    table.ForeignKey(
                        name: "FK_Experiences_Resumes_resumeID",
                        column: x => x.resumeID,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    languageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    languageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    resumeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.languageId);
                    table.ForeignKey(
                        name: "FK_Languages_Resumes_resumeID",
                        column: x => x.resumeID,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    skillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skillName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    skillType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    resumeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.skillId);
                    table.ForeignKey(
                        name: "FK_Skills_Resumes_resumeID",
                        column: x => x.resumeID,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projectts",
                columns: table => new
                {
                    projecttId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    projectDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    projectAttachements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceID = table.Column<int>(type: "int", nullable: false),
                    projectLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    portFolioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projectts", x => x.projecttId);
                    table.ForeignKey(
                        name: "FK_Projectts_PortFolios_portFolioID",
                        column: x => x.portFolioID,
                        principalTable: "PortFolios",
                        principalColumn: "portFolioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projectts_Services_serviceID",
                        column: x => x.serviceID,
                        principalTable: "Services",
                        principalColumn: "serviceId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_resumeID",
                table: "Certificates",
                column: "resumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_resumeID",
                table: "Educations",
                column: "resumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_resumeID",
                table: "Experiences",
                column: "resumeID");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_resumeID",
                table: "Languages",
                column: "resumeID");

            migrationBuilder.CreateIndex(
                name: "IX_PortFolios_EndUserId",
                table: "PortFolios",
                column: "EndUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projectts_portFolioID",
                table: "Projectts",
                column: "portFolioID");

            migrationBuilder.CreateIndex(
                name: "IX_Projectts_serviceID",
                table: "Projectts",
                column: "serviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_EndUserId",
                table: "Resumes",
                column: "EndUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_portFolioID",
                table: "Services",
                column: "portFolioID");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_resumeID",
                table: "Skills",
                column: "resumeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Projectts");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "PortFolios");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "farstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "AspNetUsers");
        }
    }
}
