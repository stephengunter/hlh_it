using ApplicationCore.DataAccess.Doc3;
using ApplicationCore.Models.Doc3;
using ApplicationCore.Specifications.Doc3;
using Infrastructure.Helpers;

namespace ApplicationCore.Services.Doc3;

public interface IReaderService
{
   Task<IEnumerable<Reader>> FetchAsync();
   Task<Reader?> GetByIdAsync(int id);
   Task<Reader> CreateAsync(Reader entity);
   Task UpdateAsync(Reader entity);
   Task AddRangeAsync(ICollection<Reader> entities);
}

public class ReaderService : IReaderService
{
	private readonly IDoc3ContextRepository<Reader> _repository;

	public ReaderService(IDoc3ContextRepository<Reader> repository)
	{
      _repository = repository;
	}
   public async Task<IEnumerable<Reader>> FetchAsync()
       => await _repository.ListAsync(new ReaderSpecification());

   public async Task<Reader?> GetByIdAsync(int id)
      => await _repository.GetByIdAsync(id);

   public async Task<Reader> CreateAsync(Reader entity)
      => await _repository.AddAsync(entity);
   public async Task AddRangeAsync(ICollection<Reader> entities)
      => await _repository.AddRangeAsync(entities);
   public async Task UpdateAsync(Reader entity)
      => await _repository.UpdateAsync(entity);

}
