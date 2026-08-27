using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using Mediator;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetAllProfissionaisByClinica
{
    public class GetAllProfissionaisByClinicaQuery : IQuery<ResultViewModel<IEnumerable<ProfissionalItemDto>>>
    {
        public GetAllProfissionaisByClinicaQuery(int page = 1, int pageSize = 20)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
