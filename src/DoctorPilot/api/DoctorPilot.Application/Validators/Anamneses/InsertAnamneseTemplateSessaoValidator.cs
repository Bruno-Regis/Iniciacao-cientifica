using DoctorPilot.Application.DTOs;
using DoctorPilot.Core.Enums;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseTemplateSessaoValidator
    : AbstractValidator<InsertAnamneseTemplateSessaoDto>
{
    public InsertAnamneseTemplateSessaoValidator(
        IValidator<InsertAnamneseTemplateOpcaoDto> opcaoValidator)
    {
        RuleFor(sessao => sessao.Pergunta)
            .NotEmpty()
            .WithMessage("Todas as perguntas devem possuir texto com no máximo 500 caracteres.");

        RuleFor(sessao => sessao.Pergunta)
            .Must(pergunta => pergunta is null || pergunta.Trim().Length <= 500)
            .WithMessage("Todas as perguntas devem possuir texto com no máximo 500 caracteres.");

        RuleFor(sessao => sessao.Tipo)
            .Must(tipo => AnamneseValidatorHelpers.TryParseTipo(tipo, out _))
            .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' possui tipo inválido.");

        RuleFor(sessao => sessao.Ordem)
            .GreaterThanOrEqualTo(0)
            .WithMessage("As perguntas devem possuir ordens não negativas e sem duplicidade.");

        RuleFor(sessao => sessao.Opcoes)
            .NotNull()
            .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' deve possuir uma lista de opções válida.");

        RuleFor(sessao => sessao.Opcoes)
            .Must(opcoes => opcoes is null || opcoes.All(opcao => opcao is not null))
            .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' possui opções inválidas.");

        When(IsTexto, () =>
        {
            RuleFor(sessao => sessao.Opcoes)
                .Must(opcoes => opcoes is null || opcoes.Count == 0)
                .WithMessage(sessao => $"A pergunta textual '{sessao.Pergunta}' não pode possuir opções.");
        });

        When(IsChoice, () =>
        {
            RuleFor(sessao => sessao.Opcoes)
                .NotEmpty()
                .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' deve possuir opções.");

            RuleFor(sessao => sessao.Opcoes)
                .Must(opcoes => opcoes is null || opcoes
                    .Where(opcao => opcao is not null)
                    .GroupBy(opcao => opcao.Ordem)
                    .All(group => group.Count() == 1))
                .WithMessage(sessao => $"As opções da pergunta '{sessao.Pergunta}' devem possuir ordens não negativas e sem duplicidade.");

            RuleFor(sessao => sessao.Opcoes)
                .Must(opcoes => opcoes is null || opcoes
                    .Where(opcao => opcao is not null)
                    .GroupBy(opcao => opcao.Texto?.Trim() ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                    .All(group => group.Count() == 1))
                .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' possui opções inválidas ou duplicadas.");

            RuleForEach(sessao => sessao.Opcoes)
                .SetValidator(opcaoValidator);
        });
    }

    private static bool IsTexto(InsertAnamneseTemplateSessaoDto sessao)
        => AnamneseValidatorHelpers.TryParseTipo(sessao.Tipo, out var tipo) &&
           tipo == AnamneseSessaoTypeEnum.texto;

    private static bool IsChoice(InsertAnamneseTemplateSessaoDto sessao)
        => AnamneseValidatorHelpers.TryParseTipo(sessao.Tipo, out var tipo) &&
           tipo is AnamneseSessaoTypeEnum.radio or AnamneseSessaoTypeEnum.checkbox;
}
