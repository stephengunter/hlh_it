using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services.Identity;
using ApplicationCore.Services.Doc3;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Helpers.Doc3.Old;
using ApplicationCore.Models.Doc3;
using ApplicationCore.Models.Identity;
using System.Xml.Linq;
using ApplicationCore.DataAccess.Doc3;

namespace IdentityWeb.Services;
public interface IDoc3Seed
{
   Task SeedPostReadersAsync();
   Task SeedPostsAsync();
   //Task SeedReadersAsync();
}
public class Doc3Seed : IDoc3Seed
{
   private readonly IOldReaderHelpers _oldReaderHelpers;
   private readonly Doc3Context _doc3Context;
   private readonly IReaderService _readerService;   
   private readonly IPostService _postService;
   

   public Doc3Seed(IOldReaderHelpers oldReaderHelpers, Doc3Context doc3Context, 
      IReaderService readerService, IPostService postService)
   {
      _oldReaderHelpers = oldReaderHelpers;
      _doc3Context = doc3Context;
      _readerService = readerService;
      _postService = postService;
   }

   public async Task SaveReaderIdAsync()
   {
      

   }
   public async Task SeedPostsAsync()
   {
      var old_posts = await _oldReaderHelpers.FetchPostsAsync();
      //  for each 100 rows, insert into db
      int batchSize = 100;

      // Loop through old_posts in batches.
      for (int i = 0; i < old_posts.Count; i += batchSize)
      {
         var batch = old_posts.Skip(i).Take(batchSize).ToList();
         await _postService.AddRangeAsync(batch);
      }
      
   }
   public async Task SeedPostReadersAsync()
   {
      var all_reader = await _readerService.FetchAsync();
      var old_readers = await _oldReaderHelpers.FetchReadersAsync();
      var contentIds = old_readers.Select(u => u.ContentId).Distinct().ToList();
      
      foreach (var contentId in contentIds)
      {
         var list = new List<PostReader>();
         var post = await _postService.FindByContentIdAsync(contentId);
         if (post == null) continue;
         var old_list = old_readers.Where(x => x.ContentId == contentId).ToList();
         foreach (var old in old_list)
         {
            var reader = all_reader.FirstOrDefault(x => x.Name == old.Name);
            var pr = new PostReader
            {
               PostId = post.Id,
               ReaderId = reader.Id,
               ViewedAt = old.ReadAt,
               Comments = old.Ps
            };
            list.Add(pr);
         }
         await _doc3Context.PostReaders.AddRangeAsync(list);
         await _doc3Context.SaveChangesAsync();
      }
   }
   //public async Task SeedReadersAsync()
   //{
   //   var all_users = await _usersService.FetchAllAsync();

   //   var old_readers = await _oldReaderHelpers.FetchReadersAsync();
   //   foreach (var old_reader in old_readers)
   //   {
   //      var user = all_users.FirstOrDefault(x => x.FullName == old_reader.Name);
   //      if (user != null)
   //      {
   //         old_reader.UserId = user.Id;
   //      }
   //   }
   //   old_readers = old_readers.DistinctBy(u => u.Name).ToList();
   //   await _readerService.AddRangeAsync(old_readers);
   //}
}

