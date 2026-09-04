using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.DTOs
{
    public class InsertAnamneseTemplateSessaoDto
    {
        public string Pergunta { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public bool Obrigatoria { get; set; }
        public List<InsertAnamneseTemplateOpcaoDto> Opcoes { get; set; } = [];
    }

    public class InsertAnamneseTemplateOpcaoDto
    {
        public string Texto { get; set; } = string.Empty;
        public int Ordem { get; set; }
    }
}
