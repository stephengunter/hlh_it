using Infrastructure.Entities;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Models.Doc3;

public class Reader : EntityBase
{
   public int? UnitId { get; set; }
   public virtual Unit? Unit { get; set; }
   public string Name { get; set; } = String.Empty;
   public string UserId { get; set; } = String.Empty;
   public string? Ps { get; set; }

   public ICollection<PostReader> PostReaders { get; set; } = new List<PostReader>();
}

