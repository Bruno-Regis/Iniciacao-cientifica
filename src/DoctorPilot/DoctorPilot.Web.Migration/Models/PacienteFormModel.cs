using System.ComponentModel.DataAnnotations;

namespace DoctorPilot.Web.Migration.Models
{
    public class PacienteFormModel
    {
        public Guid ClinicaId { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Celular { get; set; } = string.Empty;
        [Required]
        public DateTime? DataNascimento { get; set; }
        public int Genero { get; set; } = (int)GeneroEnum.Masculino;
        public int EstadoCivil { get; set; } = (int)EstadoCivilEnum.Solteiro;
        public string? NomeDaMae { get; set; }
        public string? NomeDoPai { get; set; }
        [Required]
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
        public int NecessidadeEspecial { get; set; } = (int)NecessidadeEspecialEnum.nao;
        public string? Observacoes { get; set; }
    }

    public enum EstadoCivilEnum
    {
        [Display(Name = "Solteiro(a)")] Solteiro = 0,
        [Display(Name = "Casado(a)")] Casado = 1,
        [Display(Name = "Divorciado(a)")] Divorciado = 2,
        [Display(Name = "Viúvo(a)")] Viuvo = 3,
    }

    public enum GeneroEnum
    {
        [Display(Name = "Masculino")] Masculino = 0,
        [Display(Name = "Feminino")] Feminino = 1,
        [Display(Name = "Outro")] Outro = 2
    }

    public enum NecessidadeEspecialEnum
    {
        [Display(Name = "Sim")] sim = 0,
        [Display(Name = "Não")] nao = 1
    }
}
