namespace DoctorPilot.Core.Entities
{
    public class Profissional : BaseEntity
    {
        public Profissional(Guid clinicaId, Guid usuarioId, string nome, string profissao, string? fotoUrl, string? crm)
        {
            ClinicaId = clinicaId;
            UsuarioId = usuarioId;
            Nome = nome;
            Profissao = profissao;
            FotoUrl = fotoUrl;
            Crm = crm;
        }

        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }
        private readonly List<HorarioProfissional> _horarios = [];

        public IReadOnlyCollection<HorarioProfissional> Horarios => _horarios;

        public void SetCrm(string crm)
        {
            Crm = crm;
        }
        public void SetFotoUrl(string fotoUrl)
        {
            FotoUrl = fotoUrl;
        }
        public void AdicionarHorario(HorarioProfissional horario)
        {
            _horarios.Add(horario);
        }

        public void RemoverHorario(HorarioProfissional horario)
        {
            _horarios.Remove(horario);
        }
    }
}
