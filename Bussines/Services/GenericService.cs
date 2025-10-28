using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public abstract class GenericService<TDto> : IGenericService<TDto> where TDto : BaseDto
    {
        protected readonly IGenericRepository<TDto> _repository;

        public GenericService(IGenericRepository<TDto> repository)
        {
            _repository = repository;
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entidad con ID {id} no encontrada");

            return entity;
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El objeto no puede ser nulo");

            ValidateData(dto);
            return await _repository.AddAsync(dto);
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El objeto no puede ser nulo");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var exists = await _repository.ExistsAsync(dto.Id);
            if (!exists)
                throw new KeyNotFoundException($"Entidad con ID {dto.Id} no encontrada");

            ValidateData(dto);
            return await _repository.UpdateAsync(dto);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var exists = await _repository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Entidad con ID {id} no encontrada");

            return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Método virtual para validación de datos. Debe ser sobrescrito en clases derivadas.
        /// </summary>
        protected virtual void ValidateData(TDto dto)
        {
            // Validación base vacía. Cada servicio específico la implementa según sus reglas.
        }
    }
}
