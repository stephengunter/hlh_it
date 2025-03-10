using ApplicationCore.Models.Doc3;
using ApplicationCore.Models.Identity;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Doc3;
public class ReaderSpecification : Specification<Reader>
{
   public ReaderSpecification()
   {
      
   }
   public ReaderSpecification(User user)
   {
      Query.Where(item => item.UserId == user.Id);
   }
}