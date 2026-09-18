using DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionaisByNome;
using Mediator;
using System.ComponentModel;

namespace DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant.Tools
{
    public class GetProfissionaisByNomeTool
    {
        private readonly IMediator _mediator;

        public GetProfissionaisByNomeTool(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Description("Busca profissionais da clínica atual pelo nome. Use esta ferramenta quando precisar identificar um profissional mencionado pelo usuário.")]
        public async Task<object> GetProfissionaisByNomeAsync([Description("Nome ou parte do nome do profissional.")] string nome, CancellationToken cancellationToken = default)
        {
            var query = new GetProfissionaisByNomeQuery(nome);

            return await _mediator.Send(query, cancellationToken);
        }
    }
}
