using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services.Doc3;
using ApplicationCore.Models.Identity;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Authorization;

namespace Do3Api.Controllers.Api;



public class PostsController: BaseApiController
{
   private readonly IPostService _postService;
   private readonly IReaderService _readerService;
   public PostsController(IPostService postService, IReaderService readerService)
   {
      _postService = postService;
      _readerService = readerService;
   }
   [HttpGet("init")]
   public async Task<ActionResult> Init()
   {
      var reader = await _readerService.FindByUserAsync(new User { Id = User.Id()});
      return Ok();
   }
   [HttpGet]
   public async Task<ActionResult> Index(string by = "")
   {
      
      return Ok();
   }
}

