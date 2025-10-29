using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class ModuleFormService : GenericService<ModuleFormDto>, IModuleFormService
    {
        private readonly IModuleFormRepository _moduleFormRepository;

        public ModuleFormService(IModuleFormRepository moduleFormRepository) : base(moduleFormRepository)
        {
            _moduleFormRepository = moduleFormRepository;
        }
    }
}
