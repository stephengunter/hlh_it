using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Views;

namespace ApplicationCore.Views.Docs;

public class ReaderViewModel : EntityBaseView
{
   public int ContentId { get; set; }
   public string Name { get; set; } = String.Empty;
   public string UserId { get; set; } = String.Empty;
   public string? Ps { get; set; }

   public DateTime? ReadAt { get; set; }

}
