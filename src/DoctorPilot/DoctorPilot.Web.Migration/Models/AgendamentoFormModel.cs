using System.ComponentModel.DataAnnotations;

namespace DoctorPilot.Web.Migration.Models
{
    public class AgendamentoFormModel
    {
        public Guid PacienteId { get; set; }

        public Guid ProfissionalId { get; set; }

        public TipoAgendamentoFormEnum Tipo { get; set; } = TipoAgendamentoFormEnum.Consulta;

        [Required(ErrorMessage = "Procedimento é obrigatório")]
        [StringLength(200, ErrorMessage = "Procedimento deve ter no máximo 200 caracteres")]
        public string Procedimento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data/hora inicial é obrigatória")]
        public DateTime? DataHoraInicial { get; set; }

        [Required(ErrorMessage = "Data/hora final é obrigatória")]
        public DateTime? DataHoraFinal { get; set; }

        [StringLength(50, ErrorMessage = "Sala deve ter no máximo 50 caracteres")]
        public string? Sala { get; set; }

        public bool Confirmacao { get; set; }

        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }
    }

    public enum TipoAgendamentoFormEnum
    {
        [Display(Name = "Consulta")]
        Consulta = 0,

        [Display(Name = "Exame")]
        Exame = 1,

        [Display(Name = "Procedimento")]
        Procedimento = 2,

        [Display(Name = "Retorno")]
        Retorno = 3
    }
}
