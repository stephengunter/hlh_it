using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services.Identity;
using ApplicationCore.Services.Doc3;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Helpers.Doc3.Old;
using ApplicationCore.DataAccess.Doc3;
using Polly;
using Ardalis.Specification.EntityFrameworkCore;
using ApplicationCore.Models.Doc3;
using ApplicationCore.Migrations.Doc;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{
   private readonly Doc3Context _context;

   public AATestsController(Doc3Context context)
   {
      _context = context;
   }
   [HttpGet]
   public async Task<ActionResult> Index()
   {
      var posts = _context.Posts.ToList();
      var list = new List<PostReader>();
      foreach (var post in posts)
      {
         var pr = _context.PostReaders.Find(post.Id, post.AuthorId);
         if (pr != null) list.Add(pr);
      }
      _context.PostReaders.RemoveRange(list);
      _context.SaveChanges();
      return Ok();
   }
}

