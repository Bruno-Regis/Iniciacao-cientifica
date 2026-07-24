namespace DoctorPilot.Contracts.Pacientes
{
    public class PacienteItemDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Status { get; set; }
    }
}
