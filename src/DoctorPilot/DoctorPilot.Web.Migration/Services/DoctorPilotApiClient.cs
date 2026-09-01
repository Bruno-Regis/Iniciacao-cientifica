using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Contracts.Auth;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Evolucoes;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Web.Migration.Models;
using System.Globalization;
using System.Net.Http.Json;

namespace DoctorPilot.Web.Migration.Services
{
    public class DoctorPilotApiClient 
    {
        private readonly HttpClient _httpClient;
        public DoctorPilotApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // AUTENTICAÇÃO
        public async Task<ResultViewModel<LoginResponseDto>> LoginAsync(string email, string senha)
        {
            var request = new { Email = email, Senha = senha };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<LoginResponseDto>>();
                return result ?? ResultViewModel<LoginResponseDto>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<LoginResponseDto>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        // PACIENTES
        
        public async Task<IEnumerable<PacienteItemDto>> GetAllPacientesByClinicaAsync(int page = 1, int pageSize = 20)
        {
            var result = await GetPacientesAsync(page, pageSize);
            return result.Data ?? [];
        }

        public async Task<ResultViewModel<IEnumerable<PacienteItemDto>>> GetPacientesAsync(
            int page = 1,
            int pageSize = 20)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResultViewModel<IEnumerable<PacienteItemDto>>>(
                    $"api/pacientes?page={page}&pageSize={pageSize}");
                return response ?? ResultViewModel<IEnumerable<PacienteItemDto>>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<IEnumerable<PacienteItemDto>>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<PacienteDetailsDto?> GetPacienteByIdAsync(Guid pacienteId)
        {
            var response = await _httpClient.GetFromJsonAsync<ResultViewModel<PacienteDetailsDto>>(
                $"api/pacientes/{pacienteId}");
            return response?.Data;
        }

        public async Task<ResultViewModel<Guid>> CreatePacienteAsync(PacienteFormModel command)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/pacientes", command);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>();
                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> UpdatePacienteAsync(Guid id, PacienteFormModel modelo)
        {
            var request = new UpdatePacienteRequest
            {
                PacienteId = id,
                Nome = modelo.Nome,
                Email = modelo.Email,
                Celular = modelo.Celular,
                DataNascimento = modelo.DataNascimento!.Value,
                Genero = modelo.Genero,
                EstadoCivil = modelo.EstadoCivil,
                NomeDaMae = modelo.NomeDaMae,
                NomeDoPai = modelo.NomeDoPai,
                Cpf = modelo.Cpf,
                Rg = modelo.Rg,
                OrgaoExpedidor = modelo.OrgaoExpedidor,
                Telefone = modelo.Telefone,
                CEP = modelo.CEP,
                Cidade = modelo.Cidade,
                Estado = modelo.Estado,
                Bairro = modelo.Bairro,
                Numero = modelo.Numero,
                Complemento = modelo.Complemento,
                Profissao = modelo.Profissao,
                NecessidadeEspecial = modelo.NecessidadeEspecial,
                Observacoes = modelo.Observacoes
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/pacientes/{id}", request);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>();
                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        // ALERGIAS

        public async Task<IEnumerable<AlergiaDto>> GetAllAlergiasByPacienteAsync (Guid pacienteId)
        {
            var response = await _httpClient.GetFromJsonAsync<ResultViewModel<IEnumerable<AlergiaDto>>>(
                $"api/alergias/paciente/{pacienteId}");
            return response?.Data ?? [];
        }

        public async Task<ResultViewModel<Guid>> CreateAlergiaAsync(CreateAlergiaRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/alergias", request);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>();
                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }
        public async Task<ResultViewModel<Guid>> UpdateAlergiaAsync(Guid id, AlergiaFormModel modelo)
        {
            var request = new
            {
                AlergiaId = id,
                modelo.NomeAlergia,
                Gravidade = modelo.Gravidade,
                modelo.DataIdentificacao,
                modelo.Observacoes
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/alergias/{id}", request);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>();
                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        // AGENDAMENTOS

        public async Task<ResultViewModel<IEnumerable<AgendamentoItemDto>>> GetAgendamentosByPeriodoAsync(
            DateTime inicio,
            DateTime fim,
            Guid? profissionalId = null)
        {
            var parametros = new Dictionary<string, string?>
            {
                ["inicio"] = inicio.ToString("O", CultureInfo.InvariantCulture),
                ["fim"] = fim.ToString("O", CultureInfo.InvariantCulture)
            };

            if (profissionalId.HasValue)
                parametros["profissionalId"] = profissionalId.Value.ToString();

            var queryString = Microsoft.AspNetCore.Http.QueryString.Create(parametros);

            try
            {
                var response = await _httpClient.GetAsync($"api/agendamentos/periodo{queryString}");
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<IEnumerable<AgendamentoItemDto>>>();
                return result ?? ResultViewModel<IEnumerable<AgendamentoItemDto>>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<AgendamentoDetailsDto>> GetAgendamentoByIdAsync(Guid agendamentoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/agendamentos/{agendamentoId}");
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<AgendamentoDetailsDto>>();
                return result ?? ResultViewModel<AgendamentoDetailsDto>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<AgendamentoDetailsDto>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> CreateAgendamentoAsync(AgendamentoFormModel modelo)
        {
            if (!modelo.DataHoraInicial.HasValue || !modelo.DataHoraFinal.HasValue)
                return ResultViewModel<Guid>.Error("Informe as datas/horas inicial e final.");

            var request = new
            {
                modelo.PacienteId,
                modelo.ProfissionalId,
                Tipo = (int)modelo.Tipo,
                modelo.Procedimento,
                DataHoraInicial = modelo.DataHoraInicial.Value,
                DataHoraFinal = modelo.DataHoraFinal.Value,
                modelo.Sala,
                modelo.Confirmacao,
                modelo.Descricao
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/agendamentos", request);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>();
                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> UpdateAgendamentoAsync(Guid id, AgendamentoFormModel modelo)
        {
            if (!modelo.DataHoraInicial.HasValue || !modelo.DataHoraFinal.HasValue)
                return ResultViewModel<Guid>.Error("Informe as datas/horas inicial e final.");

            var request = new
            {
                modelo.PacienteId,
                modelo.ProfissionalId,
                Tipo = (int)modelo.Tipo,
                modelo.Procedimento,
                DataHoraInicial = modelo.DataHoraInicial.Value,
                DataHoraFinal = modelo.DataHoraFinal.Value,
                modelo.Sala,
                modelo.Descricao
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/agendamentos/{id}", request);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>();
                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> CancelAgendamentoAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PatchAsync(
                    $"api/agendamentos/{id}/cancelar",
                    content: null,
                    cancellationToken: cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>(
                    cancellationToken: cancellationToken);

                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> ConfirmAgendamentoAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PatchAsync(
                    $"api/agendamentos/{id}/confirmar",
                    content: null,
                    cancellationToken: cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>(
                    cancellationToken: cancellationToken);

                return result ?? ResultViewModel<Guid>.Error("Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error("Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        // EVOLUÇÕES

        public async Task<ResultViewModel<IEnumerable<EvolucaoDto>>> GetEvolucoesByPacienteAsync(
            Guid pacienteId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"api/evolucoes/paciente/{pacienteId}",
                    cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<IEnumerable<EvolucaoDto>>>(
                    cancellationToken: cancellationToken);

                return result ?? ResultViewModel<IEnumerable<EvolucaoDto>>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<IEnumerable<EvolucaoDto>>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<EvolucaoDto>> GetEvolucaoByAgendamentoIdAsync(
            Guid agendamentoId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"api/evolucoes/agendamento/{agendamentoId}",
                    cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<EvolucaoDto>>(
                    cancellationToken: cancellationToken);

                return result ?? ResultViewModel<EvolucaoDto>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<EvolucaoDto>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> CreateEvolucaoAsync(
            Guid agendamentoId,
            string conteudo,
            CancellationToken cancellationToken = default)
        {
            var request = new CreateEvolucaoRequest
            {
                AgendamentoId = agendamentoId,
                Conteudo = conteudo
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "api/evolucoes",
                    request,
                    cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>(
                    cancellationToken: cancellationToken);

                return result ?? ResultViewModel<Guid>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        public async Task<ResultViewModel<Guid>> UpdateEvolucaoAsync(
            Guid evolucaoId,
            UpdateEvolucaoRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(
                    $"api/evolucoes/{evolucaoId}",
                    request,
                    cancellationToken);
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<Guid>>(
                    cancellationToken: cancellationToken);

                return result ?? ResultViewModel<Guid>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<Guid>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }

        // PROFISSIONAIS

        public async Task<ResultViewModel<IEnumerable<ProfissionalItemDto>>> GetProfissionaisAsync(
            int page = 1,
            int pageSize = 20)
        {
            var queryString = Microsoft.AspNetCore.Http.QueryString.Create(new Dictionary<string, string?>
            {
                ["page"] = page.ToString(CultureInfo.InvariantCulture),
                ["pageSize"] = pageSize.ToString(CultureInfo.InvariantCulture)
            });

            try
            {
                var response = await _httpClient.GetAsync($"api/profissionais{queryString}");
                var result = await response.Content.ReadFromJsonAsync<ResultViewModel<IEnumerable<ProfissionalItemDto>>>();
                return result ?? ResultViewModel<IEnumerable<ProfissionalItemDto>>.Error(
                    "Erro inesperado ao processar a resposta.");
            }
            catch (HttpRequestException)
            {
                return ResultViewModel<IEnumerable<ProfissionalItemDto>>.Error(
                    "Não foi possível conectar ao servidor. Tente novamente.");
            }
        }
    }
}
