using FluentValidation;

namespace DoctorPilot.Application.Commands.ClinicasCommands.InsertClinicas;

public sealed class InsertClinicasCommandValidator : AbstractValidator<InsertClinicasCommand>
{
    public InsertClinicasCommandValidator()
    {
        RuleFor(command => command.Nome)
            .NotEmpty().WithMessage("O nome da clínica é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da clínica deve possuir no máximo 100 caracteres.");

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("O e-mail da clínica é obrigatório.")
            .EmailAddress().WithMessage("O e-mail da clínica é inválido.")
            .MaximumLength(100).WithMessage("O e-mail da clínica deve possuir no máximo 100 caracteres.");

        RuleFor(command => command.Cnpj)
            .NotEmpty().WithMessage("O CNPJ da clínica é obrigatório.")
            .MaximumLength(14).WithMessage("O CNPJ da clínica deve possuir no máximo 14 caracteres.");

        RuleFor(command => command.AdminEmail)
            .NotEmpty().WithMessage("O e-mail do administrador é obrigatório.")
            .EmailAddress().WithMessage("O e-mail do administrador é inválido.")
            .MaximumLength(100).WithMessage("O e-mail do administrador deve possuir no máximo 100 caracteres.");

        RuleFor(command => command.AdminSenha)
            .NotEmpty().WithMessage("A senha do administrador é obrigatória.");
    }
}
