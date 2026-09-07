using System.ComponentModel.DataAnnotations;

namespace DoctorPilot.Web.Migration.Models;

public class ProfissionalCreateFormModel
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(100, ErrorMessage = "O e-mail deve possuir no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve possuir no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A profissão é obrigatória.")]
    [StringLength(100, ErrorMessage = "A profissão deve possuir no máximo 100 caracteres.")]
    public string Profissao { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "O CRM deve possuir no máximo 100 caracteres.")]
    public string? Crm { get; set; }

    public string? FotoUrl { get; set; }
}
