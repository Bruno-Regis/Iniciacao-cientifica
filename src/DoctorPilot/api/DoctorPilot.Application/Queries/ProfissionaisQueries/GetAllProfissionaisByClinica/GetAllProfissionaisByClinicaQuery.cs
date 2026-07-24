using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalByClinica
{
    public class GetAllProfissionaisByClinicaQuery : IQuery<ResultViewModel<IEnumerable<ProfissionalItemDto>>>
    {
        public GetAllProfissionaisByClinicaQuery(Guid clinicaId, int page = 1, int pageSize = 20)
        {
            ClinicaId = clinicaId;
            Page = page;
            PageSize = pageSize;
        }

        public Guid ClinicaId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
