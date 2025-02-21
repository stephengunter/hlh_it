using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Services.IT;
using System.Text;
using Infrastructure.Helpers;
using ApplicationCore.Models.IT;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{
   private readonly IItemTransactionService _service;
   private readonly IItemBalanceSheetService _balanceSheetService;
   private readonly IItemService _itemService;
   public AATestsController(IItemTransactionService service, IItemService itemService, IItemBalanceSheetService balanceSheetService)
   {
      _service = service;
      _itemService = itemService;
      _balanceSheetService = balanceSheetService;
   }

   [HttpGet]
   public async Task<ActionResult> Index(int id, int year, int month)
   {
      var date = new DateTime(year, month, 1);
      var lastDay = DateTimeHelpers.GetLastDayOfMonth(date.Year, date.Month);
      var trans = await _service.FetchAsync(date, lastDay);
      return Ok(trans);
   }

   async Task BalanceSheets(int id, int year, int month)
   {
      var item = await _itemService.GetByIdAsync(id);
      var date = new DateTime(year, month, 1);

      var lastMonth = date.AddMonths(-1);
      var lastBS = await _balanceSheetService.FindLatestAsync(item);
      while (!(lastBS?.Date.Year == lastMonth.Year && lastBS?.Date.Month == lastMonth.Month))
      {
         var bs = await CreateBalanceSheetAsync(item, lastBS);
         lastBS = bs;
      }
      var entity = await CreateBalanceSheetAsync(item, lastBS);
   }

   async Task<ItemBalanceSheet> CreateBalanceSheetAsync(Item item, ItemBalanceSheet lastBS)
   {
      var date = lastBS.Date.AddMonths(1);
      var lastDay = DateTimeHelpers.GetLastDayOfMonth(date.Year, date.Month);
      
      var trans = await _service.FetchAsync(item, date, lastDay);
      int inQty = trans.Where(x => x.Quantity > 0).Sum(x => x.Quantity);
      int outQty = Math.Abs(trans.Where(x => x.Quantity < 0).Sum(x => x.Quantity));

      
      var bs = new ItemBalanceSheet
      {
         Date = date,
         ItemId = item.Id,
         LastStock = lastBS.Quantity,
         InQty = inQty,
         OutQty = outQty
      };
      return await _balanceSheetService.CreateAsync(bs, "stephen");
   }


   async Task<List<Record>> FetchBSAsunc()
   {
      var items = await _itemService.FetchAsync();
      string filePath = @"C:\temp\BalanceSheet.txt";

      var records = new List<Record>();
      using (var reader = new StreamReader(filePath, Encoding.GetEncoding("BIG5")))
      {
         string? line;
         while ((line = reader.ReadLine()) != null)
         {
            var parts = line.SplitToList();

            var item = items.FirstOrDefault(x => x.Code == parts[0]);

           
            int year = parts[1].ToInt();
            if (year < 1) continue;
            int month = parts[2].ToInt();
            records.Add(new Record 
            { 
               Date = new DateTime(year, month, 1),
               ItemId = item.Id,
               Stock = parts[6].ToInt()
            });
         }
      }
      return records;

      //foreach (var item in items) 
      //{
      //   var rec = records.OrderBy(x => x.Date).FirstOrDefault();
      //   await _service.CreateAsync(new ItemTransaction
      //   {
      //      Date = rec.Date,
      //      ItemId = rec.ItemId,
      //      Quantity = rec.Stock,
      //      Ps = "前期結存"
      //   }, "a8748852-1b1d-473c-a2ca-fed600ef1e88");
      //}
   }
}


public class Record
{
   public int ItemId { get; set; }
   public DateTime Date { get; set; }
   public int Stock { get; set; }
}


public class Report
{
   public Report(Item item, int stock)
   {
      ItemCode= item.Code;
      ItemName= item.Name;
      Stock = stock;
   }
   public string ItemCode { get; set; }
   public string ItemName { get; set; }
   public int Stock { get; set; }
}
