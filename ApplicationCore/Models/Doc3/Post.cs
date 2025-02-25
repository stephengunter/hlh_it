using Infrastructure.Entities;
using Infrastructure.Helpers;

namespace ApplicationCore.Models.Doc3;

public class Post : EntityBase, IBaseRecord, IRemovable, ISortable
{
   public string Title { get; set; } = String.Empty;
   public string Content { get; set; } = String.Empty;
   public string Files { get; set; } = String.Empty;
   
   public string Unit { get; set; } = String.Empty;
   public string Author { get; set; } = String.Empty;
   public string? Ps { get; set; }

   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }
   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active => ISortableHelpers.IsActive(this);

}

