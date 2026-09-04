using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseFromTemplate
{
    public class InsertAnamneseFromTemplateHandler
        : ICommandHandler<InsertAnamneseFromTemplateCommand, ResultViewModel<Guid>>
    {
        private readonly IAnamneseRepository _anamneseRepository;
        private readonly IAnamneseTemplateRepository _templateRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public InsertAnamneseFromTemplateHandler(
            IAnamneseRepository anamneseRepository,
            IAnamneseTemplateRepository templateRepository,
            IPacienteRepository pacienteRepository,
            IProfissionalRepository profissionalRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _anamneseRepository = anamneseRepository;
            _templateRepository = templateRepository;
            _pacienteRepository = pacienteRepository;
            _profissionalRepository = profissionalRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(
            InsertAnamneseFromTemplateCommand command,
            CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var profissional = await _profissionalRepository.GetByUsuarioIdAsync(
                _currentUser.UsuarioId,
                cancellationToken);
            if (profissional is null || profissional.ClinicaId != paciente.ClinicaId)
                return ResultViewModel<Guid>.Error("Profissional não encontrado para a clínica do paciente.");

            var template = await _templateRepository.GetByIdComSessoesAsync(
                command.TemplateId,
                cancellationToken);
            if (template is null || template.ClinicaId != paciente.ClinicaId)
                return ResultViewModel<Guid>.Error("Template não encontrado para a clínica do paciente.");

            if (template.Sessoes.Count == 0)
                return ResultViewModel<Guid>.Error("O template não possui perguntas.");

            var perguntas = template.Sessoes.ToDictionary(s => s.Id);
            if (command.Respostas.Any(r => !perguntas.ContainsKey(r.PerguntaId)))
                return ResultViewModel<Guid>.Error("Uma ou mais perguntas não pertencem ao template informado.");

            var respostas = command.Respostas.ToDictionary(r => r.PerguntaId);
            var anamnese = new Anamnese(
                paciente.Id,
                profissional.Id,
                template.Nome,
                NormalizarOpcional(command.Observacoes),
                template.Id);

            foreach (var pergunta in template.Sessoes.OrderBy(s => s.Ordem).ThenBy(s => s.Id))
            {
                respostas.TryGetValue(pergunta.Id, out var respostaRecebida);
                var validacao = ValidarResposta(pergunta, respostaRecebida);
                if (validacao is not null)
                    return ResultViewModel<Guid>.Error(validacao);

                var opcoesSelecionadas = respostaRecebida?.OpcaoIds.ToHashSet() ?? [];
                var resposta = pergunta.Tipo == AnamneseSessaoTypeEnum.texto
                    ? respostaRecebida?.Resposta?.Trim() ?? string.Empty
                    : string.Join(
                        "; ",
                        pergunta.Opcoes
                            .Where(o => opcoesSelecionadas.Contains(o.Id))
                            .OrderBy(o => o.Ordem)
                            .ThenBy(o => o.Id)
                            .Select(o => o.Texto));

                if (pergunta.Tipo != AnamneseSessaoTypeEnum.texto && resposta.Length > 2000)
                    return ResultViewModel<Guid>.Error($"A resposta da pergunta '{pergunta.Pergunta}' excede 2000 caracteres.");

                var sessao = new AnamneseSessao(
                    anamnese.Id,
                    pergunta.Pergunta,
                    resposta,
                    pergunta.Tipo,
                    pergunta.Ordem);

                sessao.Opcoes = pergunta.Opcoes
                    .OrderBy(o => o.Ordem)
                    .ThenBy(o => o.Id)
                    .Select(o => new AnamneseSessaoOpcao(
                        sessao.Id,
                        o.Texto,
                        o.Ordem,
                        opcoesSelecionadas.Contains(o.Id)))
                    .ToList();

                anamnese.Sessoes.Add(sessao);
            }

            await _anamneseRepository.AddAsync(anamnese);
            await _unitOfWork.CommitAsync();
            return ResultViewModel<Guid>.Success(anamnese.Id);
        }

        private static string? ValidarResposta(
            AnamneseTemplateSessao pergunta,
            InsertAnamneseFromTemplateRespostaDto? resposta)
        {
            if (string.IsNullOrWhiteSpace(pergunta.Pergunta) || pergunta.Pergunta.Length > 500 || pergunta.Ordem < 0)
                return "O template possui uma pergunta inválida.";

            if (pergunta.Tipo == AnamneseSessaoTypeEnum.texto)
            {
                if (pergunta.Opcoes.Count != 0)
                    return $"A pergunta textual '{pergunta.Pergunta}' não pode possuir opções.";

                if (resposta?.OpcaoIds.Count > 0)
                    return $"A pergunta textual '{pergunta.Pergunta}' não aceita opções.";

                if (pergunta.Obrigatoria && string.IsNullOrWhiteSpace(resposta?.Resposta))
                    return $"A pergunta '{pergunta.Pergunta}' é obrigatória.";

                return null;
            }

            if (pergunta.Tipo is not (AnamneseSessaoTypeEnum.radio or AnamneseSessaoTypeEnum.checkbox))
                return $"A pergunta '{pergunta.Pergunta}' possui tipo inválido.";

            if (pergunta.Opcoes.Count == 0)
                return $"A pergunta '{pergunta.Pergunta}' deve possuir opções configuradas.";

            if (pergunta.Opcoes.Any(o => string.IsNullOrWhiteSpace(o.Texto) || o.Texto.Length > 500 || o.Ordem < 0) ||
                pergunta.Opcoes.GroupBy(o => o.Ordem).Any(g => g.Count() > 1) ||
                pergunta.Opcoes.GroupBy(o => o.Texto.Trim(), StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1))
            {
                return $"A pergunta '{pergunta.Pergunta}' possui opções configuradas de forma inválida.";
            }

            if (!string.IsNullOrWhiteSpace(resposta?.Resposta))
                return $"A pergunta '{pergunta.Pergunta}' deve ser respondida por IDs de opções.";

            var opcoesValidas = pergunta.Opcoes.Select(o => o.Id).ToHashSet();
            var selecionadas = resposta?.OpcaoIds ?? [];
            if (selecionadas.Any(id => !opcoesValidas.Contains(id)))
                return $"A pergunta '{pergunta.Pergunta}' possui uma opção inválida.";

            if (pergunta.Tipo == AnamneseSessaoTypeEnum.radio && selecionadas.Count > 1)
                return $"A pergunta '{pergunta.Pergunta}' aceita somente uma opção.";

            if (pergunta.Obrigatoria && selecionadas.Count == 0)
                return $"A pergunta '{pergunta.Pergunta}' é obrigatória.";

            return null;
        }

        private static string? NormalizarOpcional(string? valor)
            => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
