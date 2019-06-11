using FEPlus.Pattern.Infrastructure;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Pattern.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState
    {
        TEntity Find(params object[] keyValues);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void AddOrUpdateGraph(TEntity entity);
        void AddGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> Queryable();
        IRepository<T> GetRepository<T>() where T : class, IObjectState;
    }
}
