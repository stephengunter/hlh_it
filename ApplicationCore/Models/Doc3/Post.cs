using ApplicationCore.Models.IT;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Doc3;

public class Post : EntityBase, IBaseRecord, IRemovable, ISortable
{
   public int ContentId { get; set; }
   public string Title { get; set; } = String.Empty;
   public string Content { get; set; } = String.Empty;
   public string Files { get; set; } = String.Empty;


   public int UnitId { get; set; }
   [Required]
   public virtual Unit? Unit { get; set; }
   //public string UnitName { get; set; } = String.Empty;
   //public string Author { get; set; } = String.Empty;
   public int AuthorId { get; set; }

   public string? Ps { get; set; }

   public ICollection<PostReader> PostReaders { get; set; } = new List<PostReader>();

   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }
   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active => ISortableHelpers.IsActive(this);

}

