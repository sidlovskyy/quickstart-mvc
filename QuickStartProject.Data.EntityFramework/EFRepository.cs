using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;

namespace QuickStartProject.Data.EntityFramework
{
	public abstract class EFRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : DomainEntity<TId>
	{
		protected DbContext _context;		

		protected EFRepository(DbContext context)
		{
			_context = context;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
					_context = null;
				}
			}
		}

	    public IQueryable<TEntity> All()
	    {
            IQueryable<TEntity> entities = _context.Set<TEntity>();
            return entities;
	    }

	    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> @where)
	    {
            IQueryable<TEntity> entities = _context.Set<TEntity>().Where(@where);
            return entities;
	    }

	    public abstract TEntity GetById(TId id);

	    public TEntity GetOne(Expression<Func<TEntity, bool>> @where)
	    {
            TEntity entity = _context.Set<TEntity>().FirstOrDefault(@where);
            return entity;
	    }

	    public TEntity Save(TEntity entity)
	    {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entity.IsNew)
            {
                _context.Set<TEntity>().Add(entity);
            }

	        _context.SaveChanges();

	        return GetById(entity.Id);
	    }

	    public void Delete(TId id)
	    {
            TEntity entity = GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("id");
            }

            _context.Set<TEntity>().Remove(entity);

            _context.SaveChanges();
	    }
	}

    public class LongIdEFRepository<TEntity> : EFRepository<TEntity, long>
        where TEntity : DomainEntity<long>
    {
        public LongIdEFRepository(DbContext context) : base(context)
        {
        }

        public override TEntity GetById(long id)
        {
            return _context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }
    }

    public class GuidIdEFRepository<TEntity> : EFRepository<TEntity, Guid>
        where TEntity : DomainEntity<Guid>
    {
        public GuidIdEFRepository(DbContext context) : base(context)
        {
        }

        public override TEntity GetById(Guid id)
        {
            return _context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }
    }

    public class IntIdEFRepository<TEntity> : EFRepository<TEntity, int>
        where TEntity : DomainEntity<int>
    {
        public IntIdEFRepository(DbContext context) : base(context)
        {
        }

        public override TEntity GetById(int id)
        {
            return _context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }
    }
}