using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity:class
    {
        public QADB_Entities DbContext { get; private set; }
        public DbSet<TEntity> DbSet { get; private set; }
        public RepositoryBase(QADB_Entities context)
        {
            //Guard.ArgumentNotNull(context, "context");
            this.DbContext = context;
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return this.DbSet.AsQueryable();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            //Guard.ArgumentNotNull(filter, "filter");
            return this.DbSet.Where(filter).AsQueryable();
        }

        public IEnumerable<TEntity> Get<TOrderKey>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize, Expression<Func<TEntity, TOrderKey>> sortExpression, bool isAsc = true)
        {
            //Guard.ArgumentNotNull(filter, "filter");
            //Guard.ArgumentNotNull(sortExpression, "sortExpression");
            if (isAsc)
            {
                return this.DbSet.Where(filter).OrderBy(sortExpression)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
            else
            {
                return this.DbSet.Where(filter).OrderByDescending(sortExpression)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            //Guard.ArgumentNotNull(filter, "filter");
            return this.DbSet.Where(filter).Count();
        }

        public void Add(TEntity instance)
        {
            //Guard.ArgumentNotNull(instance, "instance");
            using (TransactionScope transaction = new TransactionScope())
            {
                this.DbSet.Attach(instance);
                this.DbContext.Entry(instance).State = EntityState.Added;
                this.DbContext.SaveChanges();
                transaction.Complete();
            }
        }

        public void Delete(TEntity instance)
        {
            //Guard.ArgumentNotNull(instance, "instance");
            using (TransactionScope transaction = new TransactionScope())
            {
                this.DbSet.Attach(instance);
                this.DbContext.Entry(instance).State = EntityState.Deleted;
                this.DbContext.SaveChanges();
                transaction.Complete();
            }
        }

        public void Update(TEntity instance)
        {
            //Guard.ArgumentNotNull(instance, "instance");
            using (TransactionScope transaction = new TransactionScope())
            {
				var entry = this.DbContext.Entry(instance);
				if (entry.State == EntityState.Detached)
				{
					var pkey = this.DbSet.Create().GetType().GetProperty("Id").GetValue(instance);
					TEntity attachedEntity = this.DbSet.Find(pkey);
					if (attachedEntity != null)
					{
						var attachedEntry = this.DbContext.Entry(attachedEntity);
						attachedEntry.CurrentValues.SetValues(instance);
					}
					else
					{
						//this.DbSet.Attach(instance);
						entry.State = EntityState.Modified;
					}
				}
                this.DbContext.SaveChanges();
                transaction.Complete();
            }
        }

		public Expression<Func<TEntity, bool>> GetConditionExpression<TEntity>(IList<Condition4Expression> conditionList, LogicalOperator logicalOperator)
		{
			ParameterExpression left = Expression.Parameter(typeof(TEntity), "entity");           
			Expression expression = Expression.Constant(true);
			foreach (var condition in conditionList)            
			{
				if ("" != condition.ComparisonValue)
				{
					Expression right;
					switch (condition.ComparisonMethod)
					{
						case ComparisonMethod.Contains:
							right = Expression.Call(
								Expression.Property(left, typeof(TEntity).GetProperty(condition.FieldName)),
								typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
								Expression.Constant(condition.ComparisonValue.ToString())
							);break;
						case ComparisonMethod.Equals:
							right = Expression.Equal(
								Expression.Property(left, typeof(TEntity).GetProperty(condition.FieldName)),
								Expression.Constant(condition.ComparisonValue)
							); break;
						case ComparisonMethod.NotEqual:
							right = Expression.NotEqual(
								Expression.Property(left, typeof(TEntity).GetProperty(condition.FieldName)),
								Expression.Constant(condition.ComparisonValue)
							); break;
						case ComparisonMethod.GreaterThan:
							right = Expression.GreaterThan(
								Expression.Property(left, typeof(TEntity).GetProperty(condition.FieldName)),
								Expression.Constant(condition.ComparisonValue)
							); break;
						case ComparisonMethod.LessThan:
							right = Expression.LessThan(
								Expression.Property(left, typeof(TEntity).GetProperty(condition.FieldName)),
								Expression.Constant(condition.ComparisonValue)
							); break;
						default:
							right = Expression.GreaterThanOrEqual(
								Expression.Property(left, typeof(TEntity).GetProperty(condition.FieldName)),
								Expression.Constant(condition.ComparisonValue)
							); break;
					}
					switch (logicalOperator)
					{
						case LogicalOperator.And:
							expression = Expression.And(right, expression);break;
						case LogicalOperator.Or:
							expression = Expression.Or(right, expression); break;
						default:
							break;
					}
				}
			}
			Expression<Func<TEntity, bool>> finalExpression = Expression.Lambda<Func<TEntity, bool>>(expression, new ParameterExpression[] { left });            
			return finalExpression;
		}

        public void Dispose()
        {
            this.DbContext.Dispose();
        }
    }

	public class Condition4Expression
	{
		public string FieldName { get; set; }
		public ComparisonMethod ComparisonMethod { get; set; }
		public object ComparisonValue { get; set; }
	}

	public enum LogicalOperator
	{ 
		And,
		Or,
	}

	public enum ComparisonMethod
	{
		Contains,
		Equals,
		NotEqual,
		LessThan,
		GreaterThan,
	}
}
