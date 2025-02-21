﻿using ApplicationCore.DataAccess.IT;
using ApplicationCore.Models.IT;
using ApplicationCore.Specifications.IT;

namespace ApplicationCore.Services.IT;

public interface ICategorysService
{
   Task<IEnumerable<Category>> FindRootAsync(string postType);

   Task<IEnumerable<Category>> FetchByKeysAsync(string postType, IList<string> keys);
   Task<IEnumerable<Category>> FetchAllAsync();

   Task<Category?> GetByKeyAsync(string postType, string key);
   Task<Category?> GetByIdAsync(int id);

   Task<Category> CreateAsync(Category Category);
   Task UpdateAsync(Category Category);
}

public class CategorysService : ICategorysService
{
   private readonly IITContextRepository<Category> _categorysRepository;

   public CategorysService(IITContextRepository<Category> categorysRepository)
   {
      _categorysRepository = categorysRepository;
   }
   public async Task<IEnumerable<Category>> FindRootAsync(string postType)
       => await _categorysRepository.ListAsync(new RootCategoriesSpecification(postType));
   public async Task<Category?> GetByKeyAsync(string postType, string key)
       => await _categorysRepository.FirstOrDefaultAsync(new CategoriesSpecification(postType, key));

   public async Task<IEnumerable<Category>> FetchByKeysAsync(string postType, IList<string> keys)
   => await _categorysRepository.ListAsync(new CategoriesSpecification(postType, keys));
   public async Task<IEnumerable<Category>> FetchAllAsync()
      => await _categorysRepository.ListAsync(new CategoriesSpecification());

   public async Task<Category?> GetByIdAsync(int id)
      => await _categorysRepository.GetByIdAsync(id);

   public async Task<Category> CreateAsync(Category Category)
      => await _categorysRepository.AddAsync(Category);

   public async Task UpdateAsync(Category Category)
   => await _categorysRepository.UpdateAsync(Category);

}
