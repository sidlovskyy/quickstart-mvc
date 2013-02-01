using System;
using System.Linq;
using System.Linq.Expressions;
using Logfox.Domain.Entities;
using Logfox.Domain.Repository;
using NHibernate;
using NHibernate.Linq;

namespace Logfox.Data.NHibernate
{
	public abstract class NHRepository<TEntity, TId> : IRepository<TEntity, TId>
			where TEntity : DomainEntity<TId>
	{
		protected readonly ISession _session;

		public NHRepository(ISession session)
		{
			_session = session;
		}

        public IQueryable<TEntity> All()
        {
            return _session.Query<TEntity>();
        }

	    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
		{
			return _session.Query<TEntity>()
				.Where(where);
		}

	    public abstract TEntity GetById(TId id);

		public TEntity GetOne(Expression<Func<TEntity, bool>> where)
		{
			return _session.Query<TEntity>()
				.FirstOrDefault(where);
		}
        
		public TEntity Save(TEntity entity)
		{
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    if (entity.IsNew)
                    {
                        var entityId = (TId)_session.Save(entity);
                        entity = GetById(entityId);
                    }
                    else
                    {
                        _session.Update(entity);
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return entity;
		}

        public void Delete(TId id)
		{
			using (ITransaction transaction = _session.BeginTransaction())
			{
				TEntity deleted = GetById(id);
				_session.Delete(deleted);
				transaction.Commit();
			}
		}
	}

    public class IntIdNHRepository<TEntity> : NHRepository<TEntity, int>
        where TEntity : DomainEntity<int>
    {
        public IntIdNHRepository(ISession session) : base(session) { }

        public override TEntity GetById(int id)
        {
            return _session.Query<TEntity>()
				.FirstOrDefault(e => e.Id == id);
        }
    }

    public class LongIdNHRepository<TEntity> : NHRepository<TEntity, long>
    where TEntity : DomainEntity<long>
    {
        public LongIdNHRepository(ISession session) : base(session) { }

        public override TEntity GetById(long id)
        {
            return _session.Query<TEntity>()
                .FirstOrDefault(e => e.Id == id);
        }
    }

    public class GuidIdNHRepository<TEntity> : NHRepository<TEntity, Guid>
    where TEntity : DomainEntity<Guid>
    {
        public GuidIdNHRepository(ISession session) : base(session) { }

        public override TEntity GetById(Guid id)
        {
            return _session.Query<TEntity>()
                .FirstOrDefault(e => e.Id == id);
        }
    }
}
