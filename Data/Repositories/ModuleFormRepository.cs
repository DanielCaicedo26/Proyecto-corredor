using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class ModuleFormRepository : GenericRepository<ModuleForm, ModuleFormDto>, IModuleFormRepository
    {
        public ModuleFormRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
