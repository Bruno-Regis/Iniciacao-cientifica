namespace DoctorPilot.Web.Migration.Models
{
    public class AgendaEventModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public Guid PacienteId { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public Guid ProfissionalId { get; set; }
        public string ProfissionalNome { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Procedimento { get; set; } = string.Empty;
    }
}
