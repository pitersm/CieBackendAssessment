using Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IModel
    {
        Task<IList<TEntity>> List(string navigation = null);
        IQueryable<TEntity> ListQueryable(string navigation = null);
        Task<TEntity> GetByStringProperty(string propertyName, string value);
        Task<TEntity> Get(long id, string navigation = null);
        Task<TEntity> Save(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(long id);
    }
}
