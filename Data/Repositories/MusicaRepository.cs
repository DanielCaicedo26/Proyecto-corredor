using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class MusicaRepository : GenericRepository<Musica, MusicaDto>, IMusicaRepository
    {
        public MusicaRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }


    }
}
