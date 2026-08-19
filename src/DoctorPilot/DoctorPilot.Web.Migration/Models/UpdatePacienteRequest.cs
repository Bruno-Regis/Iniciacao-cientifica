namespace DoctorPilot.Web.Migration.Models
{
    public class UpdatePacienteRequest
    {
        public Guid PacienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int Genero { get; set; }
        public int EstadoCivil { get; set; }
        public string? NomeDaMae { get; set; }
        public string? NomeDoPai { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string? Rg { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? Telefone { get; set; }
        public string CEP { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Profissao { get; set; } = string.Empty;
        public int NecessidadeEspecial { get; set; }
        public string? Observacoes { get; set; }
    }
}
