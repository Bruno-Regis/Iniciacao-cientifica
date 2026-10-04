using Microsoft.Agents.AI;
using System.Collections.Concurrent;

namespace DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant
{
    public class InMemoryAgentSessionStore
    {
        private readonly ConcurrentDictionary<Guid, AgentSession> _sessions = new();

        public Guid Create(AgentSession session)
        {
            var conversationId = Guid.NewGuid();

            _sessions[conversationId] = session;

            return conversationId;
        }

        public bool TryGet(Guid conversationId, out AgentSession? session)
        {
            return _sessions.TryGetValue(conversationId, out session);
        }
    }
}
