using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class FormaService : GenericService<FormaDto>, IFormaService
    {
        private readonly IFormaRepository _formaRepository;

        public FormaService(IFormaRepository formaRepository) : base(formaRepository)
        {
            _formaRepository = formaRepository;
        }
    }
}
