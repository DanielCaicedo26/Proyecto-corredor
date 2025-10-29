using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class ModuloRepository : GenericRepository<Modulo, ModuloDto>, IModuloRepository
    {
        public ModuloRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
