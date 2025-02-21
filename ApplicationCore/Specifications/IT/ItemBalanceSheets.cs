using ApplicationCore.Models.IT;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.IT;
public class ItemBalanceSheetSpecification : Specification<ItemBalanceSheet>
{
   public ItemBalanceSheetSpecification(Item entity)
   {
      Query.Where(x => x.ItemId == entity.Id);
   }
}
