using DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseFromTemplate;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseFromTemplateRespostaValidator
    : AbstractValidator<InsertAnamneseFromTemplateRespostaDto>
{
    public InsertAnamneseFromTemplateRespostaValidator()
    {
        RuleFor(resposta => resposta.PerguntaId)
            .NotEmpty()
            .WithMessage("A pergunta da resposta é obrigatória.");

        RuleFor(resposta => resposta.OpcaoIds)
            .NotNull()
            .WithMessage("A lista de opções selecionadas não pode ser nula.");

        RuleFor(resposta => resposta.OpcaoIds)
            .Must(opcaoIds => opcaoIds is null || opcaoIds.All(id => id != Guid.Empty))
            .WithMessage(resposta => $"A resposta da pergunta '{resposta.PerguntaId}' possui uma opção inválida.");

        RuleFor(resposta => resposta.OpcaoIds)
            .Must(opcaoIds => opcaoIds is null || opcaoIds.Distinct().Count() == opcaoIds.Count)
            .WithMessage(resposta => $"A resposta da pergunta '{resposta.PerguntaId}' possui opções selecionadas em duplicidade.");

        RuleFor(resposta => resposta.Resposta)
            .MaximumLength(2000)
            .WithMessage(resposta => $"A resposta da pergunta '{resposta.PerguntaId}' excede 2000 caracteres.");
    }
}
