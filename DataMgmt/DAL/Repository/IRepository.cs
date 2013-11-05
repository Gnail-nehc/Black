using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> Get<TOrderKey>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize,
            Expression<Func<TEntity, TOrderKey>> sortExpression, bool isAsc = true);
        int Count(Expression<Func<TEntity, bool>> filter);
        void Add(TEntity instance);
        void Delete(TEntity instance);
        void Update(TEntity instance);
		Expression<Func<TEntity, bool>> GetConditionExpression<TEntity>(IList<Condition4Expression> conditionList, LogicalOperator logicalOperator);
    }
}
