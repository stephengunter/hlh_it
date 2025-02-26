using ApplicationCore.DataAccess.Doc3;
using ApplicationCore.Models.Doc3;
using ApplicationCore.Models.Identity;
using ApplicationCore.Specifications.Doc3;
using Infrastructure.Helpers;

namespace ApplicationCore.Services.Doc3;

public interface IPostService
{
   Task<IEnumerable<Post>> FetchAsync();

   Task<IEnumerable<Post>> FetchByAuthorAsync(int authorId);
   Task<IEnumerable<Post>> FetchByReaderAsync(int readerId);
   Task<Post?> FindByContentIdAsync(int contentId);
   Task<Post?> GetByIdAsync(int id);
   Task<Post> CreateAsync(Post entity, string userId);
   Task UpdateAsync(Post entity, string userId);
   Task RemoveAsync(Post entity, string userId);

   Task UpdateRangeAsync(ICollection<Post> entities);
   Task AddRangeAsync(ICollection<Post> entities);
}

public class PostService : IPostService
{
	private readonly IDoc3ContextRepository<Post> _repository;
   private readonly Doc3Context _context;
   public PostService(IDoc3ContextRepository<Post> repository, Doc3Context context)
	{
      _repository = repository;
      _context = context;

   }
   public async Task<IEnumerable<Post>> FetchAsync()
       => await _repository.ListAsync(new PostSpecification());

   public async Task<IEnumerable<Post>> FetchByAuthorAsync(int authorId)
      => await _repository.ListAsync(new PostByAuthorSpecification(authorId));
   public async Task<IEnumerable<Post>> FetchByReaderAsync(int readerId)
   {
      var postIds = _context.PostReaders.Where(x => x.ReaderId == readerId).Select(x => x.PostId).ToList();
      return await _repository.ListAsync(new PostSpecification(postIds));
   }

   public async Task<Post?> FindByContentIdAsync(int contentId)
      => await _repository.FirstOrDefaultAsync(new PostByContentIdSpecification(contentId));

   public async Task<Post?> GetByIdAsync(int id)
      => await _repository.GetByIdAsync(id);

   public async Task<Post> CreateAsync(Post entity, string userId)
   {
      entity.SetCreated(userId);
      return await _repository.AddAsync(entity);
   }
   public async Task AddRangeAsync(ICollection<Post> entities)
      => await _repository.AddRangeAsync(entities);
   public async Task UpdateRangeAsync(ICollection<Post> entities)
      => await _repository.UpdateRangeAsync(entities);

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
