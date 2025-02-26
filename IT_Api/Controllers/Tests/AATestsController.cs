using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using ApplicationCore.Web.Controllers;
using ApplicationCore.Services.IT;
using System.Text;
using Infrastructure.Helpers;
using ApplicationCore.Models.IT;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Hosting.Server;
using ApplicationCore.Helpers;
using AutoMapper;
using System.ComponentModel;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web.Controllers.Tests;

public class AATestsController : BaseTestController
{
   private readonly IItemTransactionService _transactionService;
   private readonly IItemBalanceSheetService _balanceSheetService;
   private readonly IItemService _itemService;
   private readonly IMapper _mapper;
   public AATestsController(IItemTransactionService transactionService, IItemService itemService, 
      IItemBalanceSheetService balanceSheetService, IMapper mapper)
   {
      _transactionService = transactionService;
      _itemService = itemService;
      _balanceSheetService = balanceSheetService;
      _mapper = mapper;
   }


   [HttpGet]
   public async Task<ActionResult> Index()
   {
      var items = await _itemService.FetchAsync();
      var activeItems = items.Where(x => x.Active); 
      string filePath = @"C:\Users\Administrator\Downloads\active_items.xlsx";
      ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
      using (var excelPackage = new ExcelPackage())
      {
         var worksheet = excelPackage.Workbook.Worksheets.Add("sheet1");

         // Add headers
         worksheet.Cells[1, 1].Value = "Code";
         worksheet.Cells[1, 2].Value = "Name";
         worksheet.Cells[1, 3].Value = "Stock";

         int row = 2;
         foreach (var item in activeItems)
         {
            worksheet.Cells[row, 1].Value = item.Code;
            worksheet.Cells[row, 2].Value = item.Name;

            row++;
         }

         // Apply some basic formatting
         worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true; // Make headers bold
         worksheet.Cells.AutoFitColumns(); // Adjust column width to fit content


         // Save the Excel package to a file
         using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
         {
            excelPackage.SaveAs(stream);
         }

         Console.WriteLine($"Excel file '{filePath}' created successfully!");
      }
         //int year = 2024;
         //int month = 11;
         //var items = await _itemService.FetchAsync();
         //foreach (var item in items)
         //{
         //   if (item.Id == 2) continue;
         //   await BalanceSheets(item, year, month);
         //}
         return Ok();
   }

   async Task CreateReport(int year, int month)
   { 
      
   }
   async Task CreateFile(int id, int year, int month)
   {
      var includes = new List<string>() { nameof(Item) };
      var date = new DateTime(year, month, 1);
      var lastDay = DateTimeHelpers.GetLastDayOfMonth(date.Year, date.Month);
      var trans = await _transactionService.FetchAsync(date, lastDay, includes);
      trans = trans.GetOrdered();

      string filePath = @"C:\Users\Administrator\Downloads\trans_20250221.xlsx";
      ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
      using (var excelPackage = new ExcelPackage())
      {
         var worksheet = excelPackage.Workbook.Worksheets.Add("sheet1");

         // Add headers
         worksheet.Cells[1, 1].Value = "日期";
         worksheet.Cells[1, 2].Value = "設備編號";
         worksheet.Cells[1, 3].Value = "設備名稱";
         worksheet.Cells[1, 4].Value = "入庫/出庫";
         worksheet.Cells[1, 5].Value = "數量";
         worksheet.Cells[1, 6].Value = "科室";
         worksheet.Cells[1, 7].Value = "使用人";
         worksheet.Cells[1, 8].Value = "備註";

         int row = 2;
         foreach (var record in trans)
         {
            worksheet.Cells[row, 1].Value = record.Date.ToShortDateString();
            worksheet.Cells[row, 2].Value = record.Item!.Code;
            worksheet.Cells[row, 3].Value = record.Item!.Name;
            worksheet.Cells[row, 4].Value = record.Quantity > 0 ? "入庫" : "出庫";
            worksheet.Cells[row, 5].Value = Math.Abs(record.Quantity);
            worksheet.Cells[row, 6].Value = record.DepartmentName;
            worksheet.Cells[row, 7].Value = record.UserName;
            worksheet.Cells[row, 8].Value = record.Ps;
            row++;
         }

         // Apply some basic formatting
         worksheet.Cells[1, 1, 1, 8].Style.Font.Bold = true; // Make headers bold
         worksheet.Cells.AutoFitColumns(); // Adjust column width to fit content


         // Save the Excel package to a file
         using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
         {
            excelPackage.SaveAs(stream);
         }

         Console.WriteLine($"Excel file '{filePath}' created successfully!");
      }
   }

   async Task BalanceSheets(Item item, int year, int month)
   {
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
      
      var trans = await _transactionService.FetchAsync(item, date, lastDay);
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
      //   await _transactionService.CreateAsync(new ItemTransaction
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
