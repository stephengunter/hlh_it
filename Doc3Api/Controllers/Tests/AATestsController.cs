using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services.Identity;
using ApplicationCore.Services.Doc3;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Helpers.Doc3.Old;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{


   public AATestsController()
   {

   }
   [HttpGet]
   public async Task<ActionResult> Index()
   {
      return Ok();
   }
}

