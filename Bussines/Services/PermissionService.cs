using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class PermissionService : GenericService<PermissionDto>, IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository) : base(permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public override async Task<PermissionDto> CreateAsync(PermissionDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El permiso no puede ser nulo");

            ValidateData(dto);

            var existing = await _permissionRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new InvalidOperationException("Este nombre de permiso ya existe");

            return await _permissionRepository.AddAsync(dto);
        }

        public override async Task<PermissionDto> UpdateAsync(PermissionDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El permiso no puede ser nulo");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var existing = await _permissionRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Permiso no encontrado");

            ValidateData(dto);

            return await _permissionRepository.UpdateAsync(dto);
        }

        public async Task<PermissionDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name es requerido");

            var permission = await _permissionRepository.GetByNameAsync(name);
            if (permission == null)
                throw new KeyNotFoundException("Permiso no encontrado");

            return permission;
        }

        public async Task<List<PermissionDto>> GetPermissionsByRoleAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            return await _permissionRepository.GetPermissionsByRoleAsync(roleId);
        }

        protected override void ValidateData(PermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name es requerido");

            if (dto.Name.Length < 2)
                throw new ArgumentException("Name debe tener mínimo 2 caracteres");
        }
    }
}
