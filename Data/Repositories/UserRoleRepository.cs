using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole, UserRoleDto>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<UserRoleDto>> GetRolesByUserAsync(int userId)
        {
            var userRoles = await _context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
            return _mapper.Map<List<UserRoleDto>>(userRoles);
        }

        public async Task<List<UserRoleDto>> GetUsersByRoleAsync(int roleId)
        {
            var userRoles = await _context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.RoleId == roleId)
                .ToListAsync();
            return _mapper.Map<List<UserRoleDto>>(userRoles);
        }

        public async Task<bool> UserHasRoleAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .AsNoTracking()
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task AddAsync(UserRoleDto userRole)
        {
            var entity = _mapper.Map<UserRole>(userRole);
            entity.Updated = DateTime.UtcNow;
            await _context.UserRoles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
