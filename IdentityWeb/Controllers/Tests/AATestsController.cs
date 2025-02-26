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

namespace IdentityWeb.Controllers.Tests;

public class AATestsController : BaseTestController
{

   private readonly IDoc3Seed _doc3Seed;
   public AATestsController(IDoc3Seed doc3Seed)
   {
      _doc3Seed = doc3Seed;
   }
   [HttpGet]
   public async Task<ActionResult> Index()
   {
      
      return Ok();   
   }
}
