using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class UserRepository : GenericRepository<User, UserDto>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var user = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            var users = await _dbSet
                .AsNoTracking()
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
                .ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
