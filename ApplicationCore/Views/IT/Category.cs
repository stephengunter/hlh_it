﻿using Infrastructure.Entities;
using Infrastructure.Views;

namespace ApplicationCore.Views.IT;

public class CategoryViewModel : EntityBaseView, IBaseCategoryView<CategoryViewModel>
{
   public string EntityType { get; set; } = String.Empty;
   public string Key { get; set; } = String.Empty;
   public string Title { get; set; } = String.Empty;

   public CategoryViewModel? Parent { get; set; }

   public int? ParentId { get; set; }

   public bool IsRootItem => ParentId is null;

   public ICollection<CategoryViewModel>? SubItems { get; set; }
   public ICollection<int>? SubIds { get; set; }

   public bool Removed { get; set; }
   public int Order { get; set; }
   public bool Active { get; set; }
   
}
