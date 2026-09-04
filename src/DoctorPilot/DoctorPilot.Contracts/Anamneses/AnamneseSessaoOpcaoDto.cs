namespace DoctorPilot.Contracts.Anamneses
{
    public class AnamneseSessaoOpcaoDto
    {
        public Guid Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public bool Selecionada { get; set; }
    }
}
