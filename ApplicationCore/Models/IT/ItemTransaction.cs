using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.IT;

public class ItemTransaction : EntityBase, IBaseRecord, IRemovable
{
   public DateTime Date { get; set; }

   public int ItemId { get; set; }
   [Required]
   public virtual Item? Item { get; set; }

   public int Quantity { get; set; }

   public int? DepartmentId { get; set; }
   public string? DepartmentName { get; set; }

   public int? UserId { get; set; }
   public string? UserName { get; set; }

   public string? Ps { get; set; }
   public bool Removed { get; set; }
   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }
}
