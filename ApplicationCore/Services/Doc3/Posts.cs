using ApplicationCore.DataAccess.Doc3;
using ApplicationCore.Models.Doc3;
using ApplicationCore.Specifications.Doc3;
using Infrastructure.Helpers;

namespace ApplicationCore.Services.Doc3;

public interface IPostService
{
   Task<IEnumerable<Post>> FetchAsync();
   Task<Post?> FindByCodeAsync(string code);
   Task<Post?> GetByIdAsync(int id);
   Task<Post> CreateAsync(Post entity, string userId);
   Task UpdateAsync(Post entity, string userId);
   Task RemoveAsync(Post entity, string userId);
   Task AddRangeAsync(ICollection<Post> entities);
}

public class PostService : IPostService
{
	private readonly IDoc3ContextRepository<Post> _repository;

	public PostService(IDoc3ContextRepository<Post> repository)
	{
      _repository = repository;
	}
   public async Task<IEnumerable<Post>> FetchAsync()
       => await _repository.ListAsync(new PostSpecification());

   public async Task<Post?> FindByCodeAsync(string code)
      => await _repository.FirstOrDefaultAsync(new PostbyCodeSpecification(code));

   public async Task<Post?> GetByIdAsync(int id)
      => await _repository.GetByIdAsync(id);

   public async Task<Post> CreateAsync(Post entity, string userId)
   {
      entity.SetCreated(userId);
      return await _repository.AddAsync(entity);
   }
   public async Task AddRangeAsync(ICollection<Post> entities)
   {
      
       await _repository.AddRangeAsync(entities);
   }

   public async Task UpdateAsync(Post entity, string userId)
   {
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

   public async Task RemoveAsync(Post entity, string userId)
   {
      entity.Removed = true;
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

}
