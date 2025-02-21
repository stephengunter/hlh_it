using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Interfaces;

namespace ApplicationCore.DataAccess.Identity;
public interface IIdentityRepository<T> : IRepository<T>, IReadRepository<T> where T : class, IAggregateRoot
{
	
}
public class IdentityRepository<T> : RepositoryBase<T>, IIdentityRepository<T> where T : class, IAggregateRoot
{
	public IdentityRepository(IdentityContext dbContext) : base(dbContext)
	{
		
	}

}
