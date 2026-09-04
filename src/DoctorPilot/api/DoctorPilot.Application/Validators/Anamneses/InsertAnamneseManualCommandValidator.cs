using DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseManual;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseManualCommandValidator
    : AbstractValidator<InsertAnamneseManualCommand>
{
    public InsertAnamneseManualCommandValidator(
        IValidator<InsertAnamneseManualSessaoDto> sessaoValidator)
    {
        RuleFor(command => command.PacienteId)
            .NotEmpty()
            .WithMessage("O paciente é obrigatório.");

        RuleFor(command => command.NomeDaAnamnese)
            .NotEmpty()
            .WithMessage("O nome da anamnese é obrigatório e deve possuir no máximo 200 caracteres.");

        RuleFor(command => command.NomeDaAnamnese)
            .Must(nome => nome is null || nome.Trim().Length <= 200)
            .WithMessage("O nome da anamnese é obrigatório e deve possuir no máximo 200 caracteres.");

        RuleFor(command => command.Observacoes)
            .MaximumLength(1000)
            .WithMessage("As observações devem possuir no máximo 1000 caracteres.");

        RuleFor(command => command.Sessoes)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A lista de perguntas não pode ser nula.")
            .NotEmpty()
            .WithMessage("A anamnese deve possuir pelo menos uma pergunta.");

        RuleFor(command => command.Sessoes)
            .Must(sessoes => sessoes is null || sessoes.All(sessao => sessao is not null))
            .WithMessage("A anamnese não pode possuir perguntas nulas.");

        RuleFor(command => command.Sessoes)
            .Must(sessoes => sessoes is null || sessoes
                .Where(sessao => sessao is not null)
                .GroupBy(sessao => sessao.Ordem)
                .All(group => group.Count() == 1))
            .WithMessage("As perguntas devem possuir ordens não negativas e sem duplicidade.");

        RuleForEach(command => command.Sessoes)
            .SetValidator(sessaoValidator);
    }
}
