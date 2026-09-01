namespace DoctorPilot.Contracts.Agendamentos
{
    public class AgendamentoElegivelEvolucaoDto
    {
        public Guid Id { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string ProfissionalNome { get; set; } = string.Empty;
        public string Procedimento { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
