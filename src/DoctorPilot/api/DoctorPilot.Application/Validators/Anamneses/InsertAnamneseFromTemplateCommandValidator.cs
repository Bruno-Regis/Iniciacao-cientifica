using DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseFromTemplate;
using FluentValidation;

namespace DoctorPilot.Application.Validators.Anamneses;

public sealed class InsertAnamneseFromTemplateCommandValidator
    : AbstractValidator<InsertAnamneseFromTemplateCommand>
{
    public InsertAnamneseFromTemplateCommandValidator(
        IValidator<InsertAnamneseFromTemplateRespostaDto> respostaValidator)
    {
        RuleFor(command => command.PacienteId)
            .NotEmpty()
            .WithMessage("Paciente e template são obrigatórios.");

        RuleFor(command => command.TemplateId)
            .NotEmpty()
            .WithMessage("Paciente e template são obrigatórios.");

        RuleFor(command => command.Observacoes)
            .MaximumLength(1000)
            .WithMessage("As observações devem possuir no máximo 1000 caracteres.");

        RuleFor(command => command.Respostas)
            .NotNull()
            .WithMessage("A lista de respostas não pode ser nula.");

        RuleFor(command => command.Respostas)
            .Must(respostas => respostas is null || respostas.All(resposta => resposta is not null))
            .WithMessage("A lista de respostas não pode conter itens nulos.");

        RuleFor(command => command.Respostas)
            .Must(respostas => respostas is null || respostas
                .Where(resposta => resposta is not null)
                .GroupBy(resposta => resposta.PerguntaId)
                .All(group => group.Count() == 1))
            .WithMessage("Há respostas duplicadas para a mesma pergunta.");

        RuleForEach(command => command.Respostas)
            .SetValidator(respostaValidator);
    }
}