using FEPlus.Pattern.Repositories;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace FEPlus.Pattern.Factory
{
    public class QueryObject<TEntity> : IQueryObject<TEntity>
    {
        private Expression<Func<TEntity, bool>> _query;

        public virtual Expression<Func<TEntity, bool>> Query() => _query;

        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query) => _query = _query == null ? query : _query.And(query.Expand());

        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query) => _query = _query == null ? query : _query.Or(query.Expand());

        public Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject) => And(queryObject.Query());

        public Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject) => Or(queryObject.Query());
    }
}
