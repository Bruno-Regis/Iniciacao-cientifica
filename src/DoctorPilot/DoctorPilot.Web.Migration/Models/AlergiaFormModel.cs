using System.ComponentModel.DataAnnotations;

namespace DoctorPilot.Web.Migration.Models
{
    public class AlergiaFormModel
    {
        public Guid? AlergiaId { get; set; }

        [Required(ErrorMessage = "Nome da alergia é obrigatório")]
        public string NomeAlergia { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gravidade é obrigatória")]
        public int Gravidade { get; set; } = (int)GravidadeEnum.Baixa;

        public DateTime? DataIdentificacao { get; set; }

        public string? Observacoes { get; set; }
    }

    public enum GravidadeEnum
    {
        [Display(Name = "Baixo Risco")]
        Baixa = 0,
        [Display(Name = "Médio Risco")]
        Media = 1,
        [Display(Name = "Alto Risco")]
        Alta = 2,
        [Display(Name = "Extremo Risco")]
        Extrema = 3
    }
}
