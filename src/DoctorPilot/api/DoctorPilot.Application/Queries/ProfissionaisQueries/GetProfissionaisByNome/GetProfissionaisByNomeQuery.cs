using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionaisByNome
{
    public class GetProfissionaisByNomeQuery : IQuery<ResultViewModel<IEnumerable<ProfissionalItemDto>>>
    {
        public GetProfissionaisByNomeQuery(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
    }
}
