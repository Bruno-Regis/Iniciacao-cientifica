using DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseManual;
using DoctorPilot.Core.Enums;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseManualSessaoValidator
    : AbstractValidator<InsertAnamneseManualSessaoDto>
{
    public InsertAnamneseManualSessaoValidator(
        IValidator<InsertAnamneseManualOpcaoDto> opcaoValidator)
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

            RuleFor(sessao => sessao.Resposta)
                .NotEmpty()
                .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' exige resposta com no máximo 2000 caracteres.");

            RuleFor(sessao => sessao.Resposta)
                .Must(resposta => resposta is null || resposta.Trim().Length <= 2000)
                .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' exige resposta com no máximo 2000 caracteres.");
        });

        When(IsRadio, () =>
        {
            AddChoiceRules(opcaoValidator);

            RuleFor(sessao => sessao.Opcoes)
                .Must(opcoes => opcoes is null || opcoes.Count == 0 ||
                    opcoes.Count(opcao => opcao is not null && opcao.Selecionada) == 1)
                .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' exige exatamente uma opção selecionada.");
        });

        When(IsCheckbox, () =>
        {
            AddChoiceRules(opcaoValidator);

            RuleFor(sessao => sessao.Opcoes)
                .Must(opcoes => opcoes is null || opcoes.Count == 0 ||
                    opcoes.Any(opcao => opcao is not null && opcao.Selecionada))
                .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' exige pelo menos uma opção selecionada.");
        });
    }

    private void AddChoiceRules(IValidator<InsertAnamneseManualOpcaoDto> opcaoValidator)
    {
        RuleFor(sessao => sessao.Resposta)
            .Must(string.IsNullOrWhiteSpace)
            .WithMessage(sessao => $"A pergunta '{sessao.Pergunta}' deve ser respondida pela seleção de opções.");

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

        RuleFor(sessao => sessao.Opcoes)
            .Must(RespostaAgregadaValida)
            .WithMessage(sessao => $"A resposta da pergunta '{sessao.Pergunta}' excede 2000 caracteres.");

        RuleForEach(sessao => sessao.Opcoes)
            .SetValidator(opcaoValidator);
    }

    private static bool IsTexto(InsertAnamneseManualSessaoDto sessao)
        => AnamneseValidatorHelpers.TryParseTipo(sessao.Tipo, out var tipo) &&
           tipo == AnamneseSessaoTypeEnum.texto;

    private static bool IsRadio(InsertAnamneseManualSessaoDto sessao)
        => AnamneseValidatorHelpers.TryParseTipo(sessao.Tipo, out var tipo) &&
           tipo == AnamneseSessaoTypeEnum.radio;

    private static bool IsCheckbox(InsertAnamneseManualSessaoDto sessao)
        => AnamneseValidatorHelpers.TryParseTipo(sessao.Tipo, out var tipo) &&
           tipo == AnamneseSessaoTypeEnum.checkbox;

    private static bool RespostaAgregadaValida(List<InsertAnamneseManualOpcaoDto>? opcoes)
    {
        if (opcoes is null)
            return true;

        var resposta = string.Join(
            "; ",
            opcoes
                .Where(opcao => opcao is not null && opcao.Selecionada)
                .OrderBy(opcao => opcao.Ordem)
                .Select(opcao => opcao.Texto?.Trim() ?? string.Empty));

        return resposta.Length <= 2000;
    }
}
