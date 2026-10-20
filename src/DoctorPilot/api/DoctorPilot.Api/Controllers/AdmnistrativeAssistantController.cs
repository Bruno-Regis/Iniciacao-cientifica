using DoctorPilot.Application.AI.AdmnistrativeAssistant;
using DoctorPilot.Api.Authorization;
using DoctorPilot.Contracts.AI.AdmnistrativeAssistant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers;

[ApiController]
[Route("api/ai/administrative-assistant")]
[Authorize(Policy = AuthorizationPolicies.AdministrativeAssistantAccess)]
public class AdministrativeAssistantController : ControllerBase
{
    private readonly IAdmnistrativeAssistantService _assistantService;

    public AdministrativeAssistantController(IAdmnistrativeAssistantService assistantService)
    {
        _assistantService = assistantService;
    }

    [HttpPost]
    public async Task<IActionResult> Ask(AssistantRequest request,CancellationToken cancellationToken)
    {
        var result = await _assistantService.AskAsync(request.ConversationId, request.Message,cancellationToken);

        return Ok(new AssistantResponse
        {
            ConversationId = result.ConversationId,
            Answer = result.Answer
        });
    }
}
