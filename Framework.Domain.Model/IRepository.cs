using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Domain.Model
{
    public interface IRepository
    {
    }
    public interface IRepository<TKey,T>:IRepository where T: IAggregateRoot
    {
        T Get(Guid id);
        IList<T> GetAll();
        void DeleteAsync(T aggregate);
        Task<TKey> CreateAsync(T aggregate);
        IList<T> Get(Expression<Func<T, bool>> predicate);
    }
}
