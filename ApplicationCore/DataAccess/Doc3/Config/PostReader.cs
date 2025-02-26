using ApplicationCore.Models.Doc3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.DataAccess.Doc3.Config;
public class PostReaderConfiguration : IEntityTypeConfiguration<PostReader>
{
	public void Configure(EntityTypeBuilder<PostReader> builder)
	{
      builder.HasKey(pr => new { pr.PostId, pr.ReaderId });

      // Configure the relationship to Post
      builder.HasOne(pr => pr.Post)
             .WithMany(p => p.PostReaders)
             .HasForeignKey(pr => pr.PostId);

      // Configure the relationship to Reader
      builder.HasOne(pr => pr.Reader)
             .WithMany(r => r.PostReaders)
             .HasForeignKey(pr => pr.ReaderId);
   }
}
