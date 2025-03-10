using Infrastructure.Entities;
using Infrastructure.Helpers;

namespace ApplicationCore.Models.Doc3;

public class Unit : EntityBase, ISortable
{
   public string Name { get; set; } = String.Empty; 
   public int Order { get; set; }
   public bool Active => ISortableHelpers.IsActive(this);
   public string? Ps { get; set; }

   public ICollection<Reader> Readers { get; set; } = new List<Reader>();
}

