using System.Net.Http.Json;
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

        //Paciente

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


    }
}
