namespace DoctorPilot.Core.Entities
{
    public class AnamneseSessaoOpcao : BaseEntity
    {
        public AnamneseSessaoOpcao(Guid anamneseSessaoId, string texto, int ordem, bool selecionada)
        {
            AnamneseSessaoId = anamneseSessaoId;
            Texto = texto;
            Ordem = ordem;
            Selecionada = selecionada;
        }

        public Guid AnamneseSessaoId { get; set; }
        public AnamneseSessao AnamneseSessao { get; set; } = default!;
        public string Texto { get; set; }
        public int Ordem { get; set; }
        public bool Selecionada { get; set; }
    }
}
