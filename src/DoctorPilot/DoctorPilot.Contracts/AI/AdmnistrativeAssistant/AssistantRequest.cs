using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Contracts.AI.AdmnistrativeAssistant
{
    public class AssistantRequest
    {
        public Guid? ConversationId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
