using Infrastructure.Entities;
using Infrastructure.Helpers;

namespace ApplicationCore.Models.Doc3;

public class Reader : EntityBase
{
   public string Name { get; set; } = String.Empty;
   public string UserId { get; set; } = String.Empty;
   public string? Ps { get; set; }
}

