using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class PersonaService : GenericService<PersonaDto>, IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository) : base(personaRepository)
        {
            _personaRepository = personaRepository;
        }
    }
}
