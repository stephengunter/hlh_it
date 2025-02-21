using Infrastructure.Entities;

namespace ApplicationCore.Models.IT;

public class ItemBalanceSheet : EntityBase, IBaseRecord
{
   public DateTime Date { get; set; }

   public int ItemId { get; set; }
   public int LastStock { get; set; }
   public int InQty { get; set; }
   public int OutQty { get; set; }
   public int Quantity => LastStock + InQty - OutQty;

   public string? Ps { get; set; }
   
   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }
}
