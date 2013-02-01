using System;
using System.Linq;
using System.Linq.Expressions;
using Logfox.Domain.Entities;

namespace Logfox.Domain.Repository
{
	public interface IRepository<TEntity, TId> where TEntity : DomainEntity<TId>
	{
	    IQueryable<TEntity> All();
		IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);

        TEntity GetById(TId id);
		TEntity GetOne(Expression<Func<TEntity, bool>> where);

		TEntity Save(TEntity entity);

        void Delete(TId id);
	}
}
