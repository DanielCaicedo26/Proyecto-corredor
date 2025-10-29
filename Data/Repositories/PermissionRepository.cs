using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class PermissionRepository : GenericRepository<Permission, PermissionDto>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
