using Microsoft.AspNetCore.Mvc;
using IdentityWeb.Models;
using ApplicationCore.Consts;
using Infrastructure.Helpers;
using ApplicationCore.Authorization;
using ApplicationCore.Services.Identity;
using ApplicationCore.Models.Identity;
using ApplicationCore.Views.Identity;
using Ardalis.Specification;
using AutoMapper;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Helpers.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using IdentityWeb.Services;
using System.Security.Principal;

namespace IdentityWeb.Controllers.Tests;

public class AATestsController : BaseTestController
{
   private readonly IAppService _appService;
   private readonly IOpenIddictScopeManager _scopeManager;
   private readonly IRolesService _rolesService;
   private readonly IMapper _mapper;

   public AATestsController(IAppService appService, IRolesService rolesService, IOpenIddictScopeManager scopeManager, IMapper mapper)
   {
      _appService = appService;
      _rolesService = rolesService;
      _scopeManager = scopeManager;
      _mapper = mapper;
   }
   [HttpGet]
   public async Task<ActionResult> Index()
   {
      var app = await _appService.FindByClientIdAsync("doc3-api");
      
      

      
      //string test = _appService.GetDecryptClientSecret(app);
      return Ok();   
   }
}
