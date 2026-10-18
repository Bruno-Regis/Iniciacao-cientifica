using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseManual
{
    public class InsertAnamneseManualHandler
        : ICommandHandler<InsertAnamneseManualCommand, ResultViewModel<Guid>>
    {
        private readonly IAnamneseRepository _anamneseRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public InsertAnamneseManualHandler(
            IAnamneseRepository anamneseRepository,
            IPacienteRepository pacienteRepository,
            IProfissionalRepository profissionalRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _anamneseRepository = anamneseRepository;
            _pacienteRepository = pacienteRepository;
            _profissionalRepository = profissionalRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(
            InsertAnamneseManualCommand command,
            CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);
            if (paciente is null)
                throw new NotFoundException("Paciente não encontrado.");

            if (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId)
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var profissional = await _profissionalRepository.GetByUsuarioIdAsync(
                _currentUser.UsuarioId,
                cancellationToken);
            if (profissional is null || profissional.ClinicaId != paciente.ClinicaId)
                return ResultViewModel<Guid>.Error("Profissional não encontrado para a clínica do paciente.");

            var anamnese = new Anamnese(
                paciente.Id,
                profissional.Id,
                command.NomeDaAnamnese.Trim(),
                NormalizarOpcional(command.Observacoes));

            foreach (var pergunta in command.Sessoes.OrderBy(s => s.Ordem))
            {
                var tipo = Enum.Parse<AnamneseSessaoTypeEnum>(pergunta.Tipo, true);
                var opcoesOrdenadas = pergunta.Opcoes.OrderBy(o => o.Ordem).ToList();
                var resposta = tipo == AnamneseSessaoTypeEnum.texto
                    ? pergunta.Resposta!.Trim()
                    : string.Join("; ", opcoesOrdenadas.Where(o => o.Selecionada).Select(o => o.Texto.Trim()));

                var sessao = new AnamneseSessao(
                    anamnese.Id,
                    pergunta.Pergunta.Trim(),
                    resposta,
                    tipo,
                    pergunta.Ordem);

                sessao.Opcoes = opcoesOrdenadas
                    .Select(o => new AnamneseSessaoOpcao(
                        sessao.Id,
                        o.Texto.Trim(),
                        o.Ordem,
                        o.Selecionada))
                    .ToList();

                anamnese.Sessoes.Add(sessao);
            }

            await _anamneseRepository.AddAsync(anamnese);
            await _unitOfWork.CommitAsync();
            return ResultViewModel<Guid>.Success(anamnese.Id);
        }

        private static string? NormalizarOpcional(string? valor)
            => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
