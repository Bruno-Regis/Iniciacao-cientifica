using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorPilot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgendamentoInclusaoClinicaFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicaId",
                table: "Agendamentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_ClinicaId_DataHoraInicial",
                table: "Agendamentos",
                columns: new[] { "ClinicaId", "DataHoraInicial" });

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Clinicas_ClinicaId",
                table: "Agendamentos",
                column: "ClinicaId",
                principalTable: "Clinicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Clinicas_ClinicaId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_ClinicaId_DataHoraInicial",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "ClinicaId",
                table: "Agendamentos");
        }
    }
}
