using Infrastructure.Entities;
using Infrastructure.Helpers;
using Microsoft.Extensions.Diagnostics.Latency;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Doc3;

public class PostReader
{
   public int PostId { get; set; }

   [Required]
   public virtual Post? Post { get; set; }

   public int ReaderId { get; set; }
   [Required]
   public virtual Reader? Reader { get; set; }


   public DateTime? ViewedAt { get; set; } 
   public string? Comments { get; set; }
}

