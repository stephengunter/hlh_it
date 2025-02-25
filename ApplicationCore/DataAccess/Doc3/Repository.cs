using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Interfaces;

namespace ApplicationCore.DataAccess.Doc3;
public interface IDoc3ContextRepository<T> : IRepository<T>, IReadRepository<T> where T : class, IAggregateRoot
{
	
}
public class Doc3ContextRepository<T> : RepositoryBase<T>, IDoc3ContextRepository<T> where T : class, IAggregateRoot
{
	public Doc3ContextRepository(Doc3Context dbContext) : base(dbContext)
	{
		
	}

}
