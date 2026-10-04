namespace DoctorPilot.Application.AI.AdmnistrativeAssistant
{
    public interface IAdmnistrativeAssistantService
    {
        Task<AdministrativeAssistantResult> AskAsync(Guid? conversationId, string message, CancellationToken cancellationToken = default);
    }
}
