using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class ModuloService : GenericService<ModuloDto>, IModuloService
    {
        private readonly IModuloRepository _moduloRepository;

        public ModuloService(IModuloRepository moduloRepository) : base(moduloRepository)
        {
            _moduloRepository = moduloRepository;
        }
    }
}
