using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class RoleFormPermissionRepository : GenericRepository<RoleFormPermission, RoleFormPermissionDto>, IRoleFormPermissionRepository
    {
        public RoleFormPermissionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
