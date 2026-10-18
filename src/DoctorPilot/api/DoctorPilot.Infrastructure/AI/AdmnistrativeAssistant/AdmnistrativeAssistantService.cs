using Azure.AI.Projects;
using Azure.Identity;
using DoctorPilot.Application.AI.AdmnistrativeAssistant;
using DoctorPilot.Application.Exceptions;
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
        private readonly GetAvailableSlotsTool _availableSlotsTool;
        private readonly InMemoryAgentSessionStore _sessionStore;

        public AdmnistrativeAssistantService(IConfiguration configuration, GetProfissionaisByNomeTool profissionaisTool, GetAgendamentosTool agendamentosTool, GetAvailableSlotsTool availableSlotsTool, InMemoryAgentSessionStore sessionStore)
        {
            _configuration = configuration;
            _profissionaisTool = profissionaisTool;
            _agendamentosTool = agendamentosTool;
            _availableSlotsTool = availableSlotsTool;
            _sessionStore = sessionStore;
        }

        public async Task<AdministrativeAssistantResult> AskAsync(Guid? conversationId, string message, CancellationToken cancellationToken = default)
        {
            var endpoint = _configuration["AI:Foundry:ProjectEndpoint"] ?? throw new InvalidOperationException("Foundry ProjectEndpoint não configurado.");
            var deploymentName = _configuration["AI:Foundry:DeploymentName"] ?? throw new InvalidOperationException("Foundry DeploymentName não configurado.");
            var profissionaisTool = AIFunctionFactory.Create(_profissionaisTool.GetProfissionaisByNomeAsync);
            var agendamentosTool = AIFunctionFactory.Create(_agendamentosTool.GetAgendamentosAsync);
            var availableSlotsTool = AIFunctionFactory.Create(_availableSlotsTool.GetAvailableSlotsAsync);
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

                            Nunca invente profissionais, agendamentos, horários disponíveis
                            ou informações da clínica.

                            Quando o usuário mencionar um profissional pelo nome e for necessário
                            identificá-lo, utilize a ferramenta de busca de profissionais.

                            Para consultar agendamentos de um profissional, primeiro identifique
                            corretamente o profissional e utilize seu identificador na ferramenta
                            de consulta de agendamentos.

                            Para consultar disponibilidade de um profissional, primeiro identifique
                            corretamente o profissional e utilize seu identificador na ferramenta
                            de consulta de horários disponíveis.

                            Ao consultar disponibilidade, interprete a duração solicitada pelo
                            usuário em minutos.

                            Nunca deduza disponibilidade apenas pela ausência de agendamentos.
                            Utilize sempre a ferramenta de consulta de horários disponíveis.

                            Se mais de um profissional corresponder ao nome informado, peça ao
                            usuário que esclareça qual profissional deseja antes de continuar.

                            Você possui apenas capacidades de consulta.

                            Não ofereça criar, alterar, cancelar ou confirmar agendamentos.
                            Não solicite dados de paciente ou informações de contato com a
                            finalidade de realizar um agendamento.

                            Ao apresentar horários disponíveis, limite-se a informar os horários
                            encontrados e, se apropriado, ofereça consultar outra data, período
                            ou duração.

                            Somente afirme que pode executar uma ação quando existir uma
                            ferramenta específica disponível para realizar essa ação.

                            Não exponha identificadores internos do sistema, como IDs de
                            profissionais, pacientes, clínicas ou agendamentos, nas respostas
                            ao usuário.
                            """,
                        tools: [profissionaisTool, agendamentosTool, availableSlotsTool]);

            AgentSession agentSession;
            Guid currentConversationId;

            // Se um ConversationId for fornecido, tente recuperar a sessão existente
            if (conversationId.HasValue)
            {
                currentConversationId = conversationId.Value;

                if (!_sessionStore.TryGet(currentConversationId, out var existingSession))
                    throw new NotFoundException($"Sessão de agente não encontrada ou expirada para o ConversationId: {currentConversationId}");
                agentSession = existingSession!;
            }
            else
            {
                agentSession = await agent.CreateSessionAsync(cancellationToken: cancellationToken);

                currentConversationId =_sessionStore.Create(agentSession);
            }

            var response = await agent.RunAsync(message, agentSession, cancellationToken: cancellationToken);

            return new AdministrativeAssistantResult(currentConversationId, response.ToString());
        }
    }
}
