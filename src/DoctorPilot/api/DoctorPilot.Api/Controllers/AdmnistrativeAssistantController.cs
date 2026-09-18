using DoctorPilot.Application.AI.AdmnistrativeAssistant;
using DoctorPilot.Contracts.AI.AdmnistrativeAssistant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPilot.Api.Controllers;

[ApiController]
[Route("api/ai/administrative-assistant")]
[Authorize(Roles = "TenantAdmnin")]
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
        var answer = await _assistantService.AskAsync(request.Message,cancellationToken);

        return Ok(new AssistantResponse
        {
            Answer = answer
        });
    }
}