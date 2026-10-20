using Microsoft.Agents.AI;
using System.Collections.Concurrent;

namespace DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant
{
    internal sealed record AdministrativeAssistantSession(
        Guid ConversationId,
        Guid UsuarioId,
        Guid ClinicaId,
        AgentSession Session);

    public class InMemoryAgentSessionStore
    {
        private readonly ConcurrentDictionary<Guid, AdministrativeAssistantSession> _sessions = new();

        internal Guid Create(Guid usuarioId, Guid clinicaId, AgentSession session)
        {
            var conversationId = Guid.NewGuid();

            _sessions[conversationId] = new AdministrativeAssistantSession(
                conversationId,
                usuarioId,
                clinicaId,
                session);

            return conversationId;
        }

        internal bool TryGet(Guid conversationId, out AdministrativeAssistantSession? session)
        {
            return _sessions.TryGetValue(conversationId, out session);
        }
    }
}
