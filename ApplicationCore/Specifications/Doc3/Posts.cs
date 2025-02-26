using ApplicationCore.Models.Doc3;
using Ardalis.Specification;
using System.ComponentModel;

namespace ApplicationCore.Specifications.Doc3;
public class PostSpecification : Specification<Post>
{
   public PostSpecification()
   {
      Query.Where(item => !item.Removed);
   }
   public PostSpecification(ICollection<int> ids)
   {
      Query.Where(item => !item.Removed && ids.Contains(item.Id));
   }
}
public class PostByAuthorSpecification : Specification<Post>
{
   public PostByAuthorSpecification(int authorId)
   {
      Query.Where(item => !item.Removed && item.AuthorId == authorId);
   }
}
public class PostByContentIdSpecification : Specification<Post>
{
   public PostByContentIdSpecification(int contentId)
   {
      Query.Where(item => !item.Removed && item.ContentId == contentId);
   }
}
