using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class PersonaRepository : GenericRepository<Persona, PersonaDto>, IPersonaRepository
    {
        public PersonaRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
