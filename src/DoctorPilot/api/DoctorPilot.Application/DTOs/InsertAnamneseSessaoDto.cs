using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.DTOs
{
    public class InsertAnamneseSessaoDto
    {
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public AnamneseSessaoTypeEnum Tipo { get; set; }
    }
}
