using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class RoleService : GenericService<RoleDto>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) : base(roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public override async Task<RoleDto> CreateAsync(RoleDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El rol no puede ser nulo");

            ValidateData(dto);

            return await _roleRepository.AddAsync(dto);
        }

        public override async Task<RoleDto> UpdateAsync(RoleDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El rol no puede ser nulo");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var existing = await _roleRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Rol no encontrado");

            ValidateData(dto);

            return await _roleRepository.UpdateAsync(dto);
        }

        protected override void ValidateData(RoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name es requerido");

            if (dto.Name.Length < 2)
                throw new ArgumentException("Name debe tener mínimo 2 caracteres");
        }
    }
}
