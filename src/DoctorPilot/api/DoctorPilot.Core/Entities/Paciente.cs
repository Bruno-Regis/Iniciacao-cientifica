using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class Paciente : BaseEntity
    {
        public Paciente(Guid clinicaId, StatusEnum status, string nome, DateTime dataNascimento, GeneroEnum genero, EstadoCivilEnum estadoCivil, string? nomeDaMae, string? nomeDoPai, string cpf, string? rg, string? orgaoExpedidor, string? telefone, string celular, string email, string cEP, string cidade, string estado, string bairro, string numero, string complemento, string profissao, NecessidadeEspecialEnum necessidadeEspecial, string? observacoes)
        {
            ClinicaId = clinicaId;
            Status = status;
            Nome = nome;
            DataNascimento = dataNascimento;
            Genero = genero;
            EstadoCivil = estadoCivil;
            NomeDaMae = nomeDaMae;
            NomeDoPai = nomeDoPai;
            Cpf = cpf;
            Rg = rg;
            OrgaoExpedidor = orgaoExpedidor;
            Telefone = telefone;
            Celular = celular;
            Email = email;
            CEP = cEP;
            Cidade = cidade;
            Estado = estado;
            Bairro = bairro;
            Numero = numero;
            Complemento = complemento;
            Profissao = profissao;
            NecessidadeEspecial = necessidadeEspecial;
            Observacoes = observacoes;
        }

        public Guid ClinicaId { get; set; }
        public StatusEnum Status { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public GeneroEnum Genero { get; set; }
        public EstadoCivilEnum EstadoCivil { get; set; }
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
        public string Complemento { get; set; }
        public string Profissao { get; set; }
        public NecessidadeEspecialEnum NecessidadeEspecial { get; set; }
        public string? Observacoes { get; set; }
    }
}
