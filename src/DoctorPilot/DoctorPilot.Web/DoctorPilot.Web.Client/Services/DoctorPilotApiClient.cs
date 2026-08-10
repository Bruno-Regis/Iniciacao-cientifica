using System.Net.Http.Json;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Web.Client.Models;

namespace DoctorPilot.Web.Client.Services
{
    public class DoctorPilotApiClient
    {
        private readonly HttpClient _httpClient;
        public DoctorPilotApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // PACIENTES

        public async Task<IEnumerable<PacienteItemDto>> GetAllPacientesByClinicaAsync(Guid clinicaId, int page = 1, int pageSize = 20)
        {
            var response = await _httpClient.GetFromJsonAsync<ResultViewModel<IEnumerable<PacienteItemDto>>>(
        $"api/pacientes/clinica/{clinicaId}?page={page}&pageSize={pageSize}");

            return response?.Data ?? [];
        }

        public async Task<PacienteDetailsDto> GetPacienteByIdAsync(Guid pacienteId)
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

        public async Task<ResultViewModel<Guid>> CreateAlergiaAsync(Guid pacienteId, AlergiaFormModel modelo)
        {
            var request = new
            {
                PacienteId = pacienteId,
                modelo.NomeAlergia,
                Gravidade = modelo.Gravidade,
                modelo.DataIdentificacao,
                modelo.Observacoes
            };

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
    }
}
