using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services.Identity;
using ApplicationCore.Services.Doc3;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Helpers.Doc3.Old;

namespace IdentityWeb.Services;
public interface IDoc3Seed
{
   Task SeedReadersAsync();
}
public class Doc3Seed : IDoc3Seed
{
   private readonly IOldReaderHelpers _oldReaderHelpers;
   private readonly IUsersService _usersService;
   private readonly IReaderService _readerService;

   public Doc3Seed(IOldReaderHelpers oldReaderHelpers, IUsersService usersService, IReaderService readerService)
   {
      _oldReaderHelpers = oldReaderHelpers;
      _usersService = usersService;
      _readerService = readerService;
   }
   public async Task SeedReadersAsync()
   {
      var all_users = await _usersService.FetchAllAsync();
      //var old_readers = await _oldReaderHelpers.FetchReaders();
      //foreach (var old_reader in old_readers)
      //{
      //   var user = all_users.FirstOrDefault(x => x.FullName == old_reader.Name);
      //   if (user != null)
      //   {
      //      old_reader.UserId = user.Id;
      //   }
      //}
      //old_readers = old_readers.DistinctBy(u => u.Name).ToList();
      //await _readerService.AddRangeAsync(old_readers);
   }
}

