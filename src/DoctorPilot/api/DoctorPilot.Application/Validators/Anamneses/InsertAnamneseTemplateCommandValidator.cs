using DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.InsertAnamneseTemplate;
using DoctorPilot.Application.DTOs;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseTemplateCommandValidator
    : AbstractValidator<InsertAnamneseTemplateCommand>
{
    public InsertAnamneseTemplateCommandValidator(
        IValidator<InsertAnamneseTemplateSessaoDto> sessaoValidator)
    {
        RuleFor(command => command.Nome)
            .NotEmpty()
            .WithMessage("O nome do template é obrigatório e deve possuir no máximo 200 caracteres.");

        RuleFor(command => command.Nome)
            .Must(nome => nome is null || nome.Trim().Length <= 200)
            .WithMessage("O nome do template é obrigatório e deve possuir no máximo 200 caracteres.");

        RuleFor(command => command.Descricao)
            .MaximumLength(1000)
            .WithMessage("A descrição deve possuir no máximo 1000 caracteres.");

        RuleFor(command => command.Sessoes)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A lista de perguntas não pode ser nula.")
            .NotEmpty()
            .WithMessage("O template deve possuir pelo menos uma pergunta.");

        RuleFor(command => command.Sessoes)
            .Must(sessoes => sessoes is null || sessoes.All(sessao => sessao is not null))
            .WithMessage("O template não pode possuir perguntas nulas.");

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
