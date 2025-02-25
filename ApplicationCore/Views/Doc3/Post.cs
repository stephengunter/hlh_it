using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Views;

namespace ApplicationCore.Views.Docs;

public class PostViewModel : EntityBaseView, IBaseRecordView
{
   public string Title { get; set; } = String.Empty;
   public string Content { get; set; } = String.Empty;
   public string Files { get; set; } = String.Empty;
   public List<string> FileList => Files.SplitToList();

   public string Unit { get; set; } = String.Empty;
   public string Author { get; set; } = String.Empty;
   public string? Ps { get; set; }

   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active { get; set; }

   public DateTime CreatedAt { get; set; }
   public string CreatedBy { get; set; } = String.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }

   public string CreatedAtText => CreatedAt.ToDateTimeString();
   public string LastUpdatedText => LastUpdated.ToDateTimeString();

}
