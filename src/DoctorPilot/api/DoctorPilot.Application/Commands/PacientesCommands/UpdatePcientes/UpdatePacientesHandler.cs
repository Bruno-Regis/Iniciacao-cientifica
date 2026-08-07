using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.PacientesCommands.UpdatePcientes
{
    public class UpdatePacientesHandler : ICommandHandler<UpdatePacientesCommand, ResultViewModel<Guid>>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePacientesHandler(IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork)
        {
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(UpdatePacientesCommand command, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);

            if (paciente == null) 
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            paciente.Nome = command.Nome;
            paciente.DataNascimento = command.DataNascimento;
            paciente.Genero = command.Genero;
            paciente.EstadoCivil = command.EstadoCivil;
            paciente.NomeDaMae = command.NomeDaMae;
            paciente.NomeDoPai = command.NomeDoPai;
            paciente.Cpf = command.Cpf;
            paciente.Rg = command.Rg;
            paciente.OrgaoExpedidor = command.OrgaoExpedidor;
            paciente.Telefone = command.Telefone;
            paciente.Celular = command.Celular;
            paciente.Email = command.Email;
            paciente.CEP = command.CEP;
            paciente.Cidade = command.Cidade;
            paciente.Estado = command.Estado;
            paciente.Bairro = command.Bairro;
            paciente.Numero = command.Numero;
            paciente.Complemento = command.Complemento;
            paciente.Profissao = command.Profissao;
            paciente.NecessidadeEspecial = command.NecessidadeEspecial;
            paciente.Observacoes = command.Observacoes;

            await _pacienteRepository.UpdateAsync(paciente);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(paciente.Id);
        }
    }
}
