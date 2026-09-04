using DoctorPilot.Application.DTOs;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseTemplateOpcaoValidator
    : AbstractValidator<InsertAnamneseTemplateOpcaoDto>
{
    public InsertAnamneseTemplateOpcaoValidator()
    {
        RuleFor(opcao => opcao.Texto)
            .NotEmpty()
            .WithMessage("Todas as opções devem possuir texto com no máximo 500 caracteres.");

        RuleFor(opcao => opcao.Texto)
            .Must(texto => texto is null || texto.Trim().Length <= 500)
            .WithMessage("Todas as opções devem possuir texto com no máximo 500 caracteres.");

        RuleFor(opcao => opcao.Ordem)
            .GreaterThanOrEqualTo(0)
            .WithMessage("As opções devem possuir ordens não negativas e sem duplicidade.");
    }
}
