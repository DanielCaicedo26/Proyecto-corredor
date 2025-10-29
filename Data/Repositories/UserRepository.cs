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
    }
}
