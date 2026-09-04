using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorPilot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImproveAnamneseTemplateStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Obrigatoria",
                table: "AnamneseTemplateSessoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "AnamneseTemplateSessoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "AnamneseSessoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AnamneseTemplateId",
                table: "Anamneses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnamneseSessaoOpcoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnamneseSessaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Selecionada = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseSessaoOpcoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseSessaoOpcoes_AnamneseSessoes_AnamneseSessaoId",
                        column: x => x.AnamneseSessaoId,
                        principalTable: "AnamneseSessoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnamneseTemplateOpcoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnamneseTemplateSessaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseTemplateOpcoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseTemplateOpcoes_AnamneseTemplateSessoes_AnamneseTemplateSessaoId",
                        column: x => x.AnamneseTemplateSessaoId,
                        principalTable: "AnamneseTemplateSessoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anamneses_AnamneseTemplateId",
                table: "Anamneses",
                column: "AnamneseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseSessaoOpcoes_AnamneseSessaoId_Ordem",
                table: "AnamneseSessaoOpcoes",
                columns: new[] { "AnamneseSessaoId", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseTemplateOpcoes_AnamneseTemplateSessaoId_Ordem",
                table: "AnamneseTemplateOpcoes",
                columns: new[] { "AnamneseTemplateSessaoId", "Ordem" });

            migrationBuilder.AddForeignKey(
                name: "FK_Anamneses_AnamneseTemplates_AnamneseTemplateId",
                table: "Anamneses",
                column: "AnamneseTemplateId",
                principalTable: "AnamneseTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anamneses_AnamneseTemplates_AnamneseTemplateId",
                table: "Anamneses");

            migrationBuilder.DropTable(
                name: "AnamneseSessaoOpcoes");

            migrationBuilder.DropTable(
                name: "AnamneseTemplateOpcoes");

            migrationBuilder.DropIndex(
                name: "IX_Anamneses_AnamneseTemplateId",
                table: "Anamneses");

            migrationBuilder.DropColumn(
                name: "Obrigatoria",
                table: "AnamneseTemplateSessoes");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "AnamneseTemplateSessoes");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "AnamneseSessoes");

            migrationBuilder.DropColumn(
                name: "AnamneseTemplateId",
                table: "Anamneses");
        }
    }
}
