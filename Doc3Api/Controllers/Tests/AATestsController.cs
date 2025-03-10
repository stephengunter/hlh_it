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

namespace Do3Api.Controllers.Tests;

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
      //var units = _context.Units.ToList();
      //var posts = _context.Posts.ToList();
      //foreach (var post in posts)
      //{
      //   var unit = units.FirstOrDefault(x => x.Name == post.UnitName.Trim());
      //   if (unit == null) continue;
      //   post.UnitId = unit.Id;
      //}
     
      //_context.SaveChanges();
      return Ok();
   }
}

