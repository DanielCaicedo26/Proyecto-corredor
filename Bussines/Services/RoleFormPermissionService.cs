using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class RoleFormPermissionService : GenericService<RoleFormPermissionDto>, IRoleFormPermissionService
    {
        private readonly IRoleFormPermissionRepository _roleFormPermissionRepository;

        public RoleFormPermissionService(IRoleFormPermissionRepository roleFormPermissionRepository) : base(roleFormPermissionRepository)
        {
            _roleFormPermissionRepository = roleFormPermissionRepository;
        }

        public async Task<RoleFormPermissionDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var roleFormPermission = await _roleFormPermissionRepository.GetByIdAsync(id);
            if (roleFormPermission == null)
                throw new KeyNotFoundException($"RoleFormPermission con ID {id} no encontrado");

            return roleFormPermission;
        }

        public async Task<List<RoleFormPermissionDto>> GetAllAsync()
        {
            return await _roleFormPermissionRepository.GetAllAsync();
        }

        public async Task<RoleFormPermissionDto> CreateAsync(RoleFormPermissionDto dto)
        {
            ValidateRoleFormPermissionData(dto);
            return await _roleFormPermissionRepository.AddAsync(dto);
        }

        public async Task<RoleFormPermissionDto> UpdateAsync(RoleFormPermissionDto dto)
        {
            var existing = await _roleFormPermissionRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("RoleFormPermission no encontrado");

            ValidateRoleFormPermissionData(dto);
            return await _roleFormPermissionRepository.UpdateAsync(dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _roleFormPermissionRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("RoleFormPermission no encontrado");

            return await _roleFormPermissionRepository.DeleteAsync(id);
        }

        public async Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAndFormaAsync(int roleId, int formaId)
        {
            if (roleId <= 0 || formaId <= 0)
                throw new ArgumentException("Role ID y Forma ID deben ser mayores a 0");

            return await _roleFormPermissionRepository.GetPermissionsByRoleAndFormaAsync(roleId, formaId);
        }

        public async Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            return await _roleFormPermissionRepository.GetPermissionsByRoleAsync(roleId);
        }

        public async Task<List<RoleFormPermissionDto>> GetPermissionsByFormaAsync(int formaId)
        {
            if (formaId <= 0)
                throw new ArgumentException("Forma ID debe ser mayor a 0");

            return await _roleFormPermissionRepository.GetPermissionsByFormaAsync(formaId);
        }

        private void ValidateRoleFormPermissionData(RoleFormPermissionDto dto)
        {
            if (dto.RoleId <= 0)
                throw new ArgumentException("RoleId es requerido");

            if (dto.FormaId <= 0)
                throw new ArgumentException("FormaId es requerido");

            if (dto.PermissionId <= 0)
                throw new ArgumentException("PermissionId es requerido");
        }
    }
}
