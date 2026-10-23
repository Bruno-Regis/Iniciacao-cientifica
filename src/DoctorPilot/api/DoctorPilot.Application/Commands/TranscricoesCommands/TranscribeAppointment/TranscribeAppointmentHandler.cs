using DoctorPilot.Application.AI.Transcription;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Transcricoes;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.TranscricoesCommands.TranscribeAppointment
{
    public class TranscribeAppointmentHandler : IRequestHandler<TranscribeAppointmentCommand, TranscribeAppointmentResponse>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ITranscriptionService _transcriptionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Transcricao> _transcricaoRepository;

        public TranscribeAppointmentHandler(IAgendamentoRepository agendamentoRepository, IProfissionalRepository profissionalRepository, ITranscriptionService transcriptionService, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IRepository<Transcricao> transcricaoRepository)
        {       
            _agendamentoRepository = agendamentoRepository;
            _transcriptionService = transcriptionService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _transcricaoRepository = transcricaoRepository;
            _profissionalRepository = profissionalRepository;
        }

        public async ValueTask<TranscribeAppointmentResponse> Handle(TranscribeAppointmentCommand command, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUserService.ClinicaId
               ?? throw new ForbiddenException(
                   "Usuário não possui vínculo com uma clínica.");

            var profissional = await _profissionalRepository.GetByUsuarioIdAsync(
                _currentUserService.UsuarioId,
                cancellationToken);

            if (profissional is null)
            {
                throw new ForbiddenException(
                    "Profissional não identificado para o usuário autenticado.");
            }

            var agendamento = await _agendamentoRepository.GetByIdAsync(
                command.AgendamentoId);

            if (agendamento is null)
            {
                throw new NotFoundException(
                    "Agendamento não encontrado.");
            }

            if (agendamento.ClinicaId != clinicaId)
            {
                throw new NotFoundException(
                    "Agendamento não encontrado.");
            }

            if (agendamento.ProfissionalId != profissional.Id)
            {
                throw new ForbiddenException(
                    "O profissional autenticado não possui acesso a este agendamento.");
            }

            var transcriptionResult =
                await _transcriptionService.TranscribeAsync(
                    command.Audio,
                    command.FileName,
                    cancellationToken);

            var textoTranscrito = string.Join(
                Environment.NewLine,
                transcriptionResult.Segments.Select(segment =>
                    $"Speaker {segment.Speaker}: {segment.Text}"));

            var transcricao = new Transcricao(
                agendamento.Id,
                textoTranscrito);

            await _transcricaoRepository.AddAsync(transcricao);

            await _unitOfWork.CommitAsync();

            return new TranscribeAppointmentResponse
            {
                TranscricaoId = transcricao.Id,
                TextoTranscrito = transcricao.TextoTranscrito
            };
        }
    }
}
