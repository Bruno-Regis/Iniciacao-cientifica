using FluentValidation;

namespace DoctorPilot.Application.Commands.TranscricoesCommands.TranscribeAppointment;

public sealed class TranscribeAppointmentCommandValidator : AbstractValidator<TranscribeAppointmentCommand>
{
    public TranscribeAppointmentCommandValidator()
    {
        RuleFor(command => command.AgendamentoId)
            .NotEmpty().WithMessage("O agendamento é obrigatório.");

        RuleFor(command => command.Audio)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("O áudio é obrigatório.")
            .Must(BeUsableAudio).WithMessage("O áudio deve ser um stream legível e com conteúdo.");

        RuleFor(command => command.FileName)
            .NotEmpty().WithMessage("O nome do arquivo de áudio é obrigatório.");
    }

    private static bool BeUsableAudio(Stream? audio)
    {
        if (audio is null || audio == Stream.Null || !audio.CanRead)
        {
            return false;
        }

        if (!audio.CanSeek)
        {
            return true;
        }

        try
        {
            return audio.Position < audio.Length;
        }
        catch (NotSupportedException)
        {
            return false;
        }
        catch (ObjectDisposedException)
        {
            return false;
        }
    }
}
