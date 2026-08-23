using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorPilot.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexEvolucaoAgendamentoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evolucoes_AgendamentoId",
                table: "Evolucoes");

            migrationBuilder.CreateIndex(
                name: "IX_Evolucoes_AgendamentoId",
                table: "Evolucoes",
                column: "AgendamentoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evolucoes_AgendamentoId",
                table: "Evolucoes");

            migrationBuilder.CreateIndex(
                name: "IX_Evolucoes_AgendamentoId",
                table: "Evolucoes",
                column: "AgendamentoId");
        }
    }
}
