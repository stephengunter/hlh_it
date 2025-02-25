using ApplicationCore.Models.Doc3;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Doc3;
public class PostSpecification : Specification<Post>
{
   public PostSpecification()
   {
      Query.Where(item => !item.Removed);
   }
}
public class PostbyCodeSpecification : Specification<Post>
{
   public PostbyCodeSpecification(string author)
   {
      Query.Where(item => !item.Removed && item.Author == author);
   }
}
