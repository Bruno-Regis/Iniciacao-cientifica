using FluentValidation;

namespace DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais;

public sealed class InsertProfissionaisCommandValidator : AbstractValidator<InsertProfissionaisCommand>
{
    public InsertProfissionaisCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("O e-mail do profissional é obrigatório.")
            .EmailAddress().WithMessage("O e-mail do profissional é inválido.")
            .MaximumLength(100).WithMessage("O e-mail do profissional deve possuir no máximo 100 caracteres.");

        RuleFor(command => command.Senha)
            .NotEmpty().WithMessage("A senha do profissional é obrigatória.");

        RuleFor(command => command.Nome)
            .NotEmpty().WithMessage("O nome do profissional é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do profissional deve possuir no máximo 100 caracteres.");

        RuleFor(command => command.Profissao)
            .NotEmpty().WithMessage("A profissão do profissional é obrigatória.")
            .MaximumLength(100).WithMessage("A profissão do profissional deve possuir no máximo 100 caracteres.");

        RuleFor(command => command.Crm)
            .MaximumLength(100).WithMessage("O CRM do profissional deve possuir no máximo 100 caracteres.");
    }
}
