using ELBORAI.Application.Specifications;
using ELBORAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public  Task<T?> GetByIdAsync(int id);

        public Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetBySpecificationAsync(
         BaseSpecification<T> specification);

        Task<IReadOnlyList<T>> GetAllBySpecificationAsync(
        BaseSpecification<T> specification);

        public  Task AddAsync(T entity);

        public void Update(T entity);

        public void Delete(T entity);




    }
}
