using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services.Doc3;
using ApplicationCore.Web.Controllers;

namespace Web.Controllers.Tests;

public class PostsController : BaseApiController
{
   private readonly IPostService _postService;
   public PostsController(IPostService postService)
   {
      _postService = postService;
   }
   [HttpGet("i" +
      "nit")]
   public async Task<ActionResult> Init()
   {

      return Ok();
   }
   [HttpGet]
   public async Task<ActionResult> Index(string by = "")
   {
      
      return Ok();
   }
}

