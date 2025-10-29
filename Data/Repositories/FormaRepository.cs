using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class FormaRepository : GenericRepository<Forma, FormaDto>, IFormaRepository
    {
        public FormaRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
