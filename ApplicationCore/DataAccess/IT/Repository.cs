using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Interfaces;

namespace ApplicationCore.DataAccess.IT;
public interface IITContextRepository<T> : IRepository<T>, IReadRepository<T> where T : class, IAggregateRoot
{
	
}
public class ITContextRepository<T> : RepositoryBase<T>, IITContextRepository<T> where T : class, IAggregateRoot
{
	public ITContextRepository(ITContext dbContext) : base(dbContext)
	{
		
	}

}
