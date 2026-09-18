using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.AI.AdmnistrativeAssistant
{
    public interface IAdmnistrativeAssistantService
    {
        Task<string> AskAsync(string message, CancellationToken cancellationToken = default);
    }
}
