using ApplicationCore.DataAccess.IT;
using ApplicationCore.Models.IT;
using ApplicationCore.Specifications.IT;
using Ardalis.Specification;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;

namespace ApplicationCore.Services.IT;

public interface IItemBalanceSheetService
{
   Task<IEnumerable<ItemBalanceSheet>> FetchAsync(Item entity);
   Task<ItemBalanceSheet?> FindLatestAsync(Item entity);
   Task<ItemBalanceSheet?> GetByIdAsync(int id);
   Task<ItemBalanceSheet> CreateAsync(ItemBalanceSheet entity, string userId);
   Task UpdateAsync(ItemBalanceSheet entity, string userId);
   Task DeleteAsync(ItemBalanceSheet entity);
   Task AddRangeAsync(ICollection<ItemBalanceSheet> entities);
}

public class ItemBalanceSheetService : IItemBalanceSheetService
{
	private readonly IITContextRepository<ItemBalanceSheet> _repository;

	public ItemBalanceSheetService(IITContextRepository<ItemBalanceSheet> repository)
	{
      _repository = repository;
	}
   public async Task<IEnumerable<ItemBalanceSheet>> FetchAsync(Item entity)
       => await _repository.ListAsync(new ItemBalanceSheetSpecification(entity));

   public async Task<ItemBalanceSheet?> FindLatestAsync(Item entity)
   {
      var list = await FetchAsync(entity);
      if (list.IsNullOrEmpty()) return null;
      return list?.OrderByDescending(x => x.Date).FirstOrDefault();
   }
   public async Task<ItemBalanceSheet?> GetByIdAsync(int id)
      => await _repository.GetByIdAsync(id);

   public async Task<ItemBalanceSheet> CreateAsync(ItemBalanceSheet entity, string userId)
   {
      entity.SetCreated(userId);
      return await _repository.AddAsync(entity);
   }
   public async Task AddRangeAsync(ICollection<ItemBalanceSheet> entities)
   {
      
       await _repository.AddRangeAsync(entities);
   }

   public async Task UpdateAsync(ItemBalanceSheet entity, string userId)
   {
      entity.SetUpdated(userId);
      await _repository.UpdateAsync(entity);
   }

   public async Task DeleteAsync(ItemBalanceSheet entity)
      => await _repository.DeleteAsync(entity);
}
