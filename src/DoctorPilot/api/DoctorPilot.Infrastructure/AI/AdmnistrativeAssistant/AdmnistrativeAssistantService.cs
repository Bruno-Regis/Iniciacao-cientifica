using Azure.AI.Projects;
using Azure.Identity;
using DoctorPilot.Application.AI.AdmnistrativeAssistant;
using DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

namespace DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant
{
    public class AdmnistrativeAssistantService : IAdmnistrativeAssistantService
    {

        private readonly IConfiguration _configuration;
        private readonly GetProfissionaisByNomeTool _profissionaisTool;
        private readonly GetAgendamentosTool _agendamentosTool;

        public AdmnistrativeAssistantService(IConfiguration configuration, GetProfissionaisByNomeTool profissionaisTool, GetAgendamentosTool agendamentosTool)
        {
            _configuration = configuration;
            _profissionaisTool = profissionaisTool;
            _agendamentosTool = agendamentosTool;
        }

        public async Task<string> AskAsync(string message, CancellationToken cancellationToken = default)
        {
            var endpoint = _configuration["AI:Foundry:ProjectEndpoint"] ?? throw new InvalidOperationException("Foundry ProjectEndpoint não configurado.");
            var deploymentName = _configuration["AI:Foundry:DeploymentName"] ?? throw new InvalidOperationException("Foundry DeploymentName não configurado.");

            var profissionaisTool = AIFunctionFactory.Create(_profissionaisTool.GetProfissionaisByNomeAsync);
            var agendamentosTool = AIFunctionFactory.Create(_agendamentosTool.GetAgendamentosAsync);
            var agora = DateTimeOffset.Now;
            AIAgent agent = new AIProjectClient(
                new Uri(endpoint),
                new AzureCliCredential()) 
                    .AsAIAgent(
                        model: deploymentName,
                        name: "AdministrativeAssistant",
                        instructions:
                            $"""
                            Você é o assistente administrativo do DoctorPilot.

                            A data e hora atual do sistema é:
                            {agora:dd/MM/yyyy HH:mm}.

                            Ao interpretar expressões relativas como "hoje", "amanhã",
                            "ontem" ou "próxima semana", utilize essa data como referência.

                            Utilize as ferramentas disponíveis para consultar informações
                            reais do sistema.

                            Nunca invente profissionais, agendamentos ou informações da clínica.

                            Quando o usuário mencionar um profissional pelo nome e for necessário
                            identificá-lo, utilize a ferramenta de busca de profissionais.

                            Para consultar agendamentos de um profissional, primeiro identifique
                            corretamente o profissional e utilize seu identificador na ferramenta
                            de consulta de agendamentos.

                            Se mais de um profissional corresponder ao nome informado, peça ao
                            usuário que esclareça qual profissional deseja antes de consultar
                            seus agendamentos.

                            Não afirme nem ofereça funcionalidades que não estejam disponíveis
                            por meio das ferramentas fornecidas.
                            """,
                        tools: [profissionaisTool, agendamentosTool]);

            var response = await agent.RunAsync(message, cancellationToken: cancellationToken);

            return response.ToString();
        }
    }
}
