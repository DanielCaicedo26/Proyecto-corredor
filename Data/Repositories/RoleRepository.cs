using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class RoleRepository : GenericRepository<Role, RoleDto>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
