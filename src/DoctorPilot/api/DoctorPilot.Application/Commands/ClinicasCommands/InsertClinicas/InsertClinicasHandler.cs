using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;
using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.Commands.ClinicasCommands.InsertClinicas
{
    public class InsertClinicasHandler : ICommandHandler<InsertClinicasCommand, ResultViewModel<Guid>>
    {
        private readonly IClinicaRepository _clinicaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public InsertClinicasHandler(
            IClinicaRepository clinicaRepository,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _clinicaRepository = clinicaRepository;
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertClinicasCommand command, CancellationToken cancellationToken)
        {
            if (await _clinicaRepository.ExistsCnpjAsync(command.Cnpj))
                throw new ConflictException("Já existe uma clínica com este CNPJ.");

            if (await _clinicaRepository.ExistsEmailAsync(command.Email))
                throw new ConflictException("Já existe uma clínica com este e-mail.");

            if (await _usuarioRepository.ExistsEmailAsync(command.AdminEmail))
                throw new ConflictException("Já existe um usuário com este e-mail.");

            var clinica = command.ToEntity();
            var senhaHash = _passwordHasher.Hash(command.AdminSenha);
            var administrador = new Usuario(
                clinica.Id,
                command.AdminEmail,
                senhaHash,
                RoleEnum.TenantAdmnin);

            await _clinicaRepository.AddAsync(clinica);
            await _usuarioRepository.AddAsync(administrador);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(clinica.Id);
        }
    }
}
