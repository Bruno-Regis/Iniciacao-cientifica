using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Contracts.AI.AdmnistrativeAssistant
{
    public class AssistantResponse
    {
        public Guid? ConversationId { get; set; }
        public string Answer { get; set; } = string.Empty;
    }
}
