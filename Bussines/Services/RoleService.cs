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

            var existing = await _roleRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new InvalidOperationException("Este nombre de rol ya existe");

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

        public async Task<RoleDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name es requerido");

            var role = await _roleRepository.GetByNameAsync(name);
            if (role == null)
                throw new KeyNotFoundException("Rol no encontrado");

            return role;
        }

        public async Task<List<RoleDto>> GetRolesByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID debe ser mayor a 0");

            return await _roleRepository.GetRolesByUserAsync(userId);
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
