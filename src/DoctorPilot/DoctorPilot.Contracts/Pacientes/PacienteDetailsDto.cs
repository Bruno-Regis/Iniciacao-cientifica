namespace DoctorPilot.Contracts.Pacientes
{
    public class PacienteDetailsDto
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string? NomeDaMae { get; set; }
        public string? NomeDoPai { get; set; }
        public string Cpf { get; set; }
        public string? Rg { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string Profissao { get; set; }
        public string NecessidadeEspecial { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
