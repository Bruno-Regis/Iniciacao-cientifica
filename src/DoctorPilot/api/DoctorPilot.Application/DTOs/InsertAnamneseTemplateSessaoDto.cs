using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.DTOs
{
    public class InsertAnamneseTemplateSessaoDto
    {
        public string Pergunta { get; set; }
        public AnamneseSessaoTypeEnum Tipo { get; set; }
    }
}
