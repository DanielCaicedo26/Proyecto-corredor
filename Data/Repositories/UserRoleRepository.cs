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
    }
}
