﻿using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models.Identity;

public class User : IdentityUser, IAggregateRoot, IBaseRecord
{	
	public string Name { get; set; } = String.Empty;
   public DateTime CreatedAt { get; set; } = DateTime.Now;
   public string CreatedBy { get; set; } = string.Empty;
   public DateTime? LastUpdated { get; set; }
   public string? UpdatedBy { get; set; }
   public bool Active { get; set; }
   public virtual Profiles? Profiles { get; set; }

   public virtual ICollection<UserRole>? UserRoles { get; set; }

   public object GetId() => Id;

   [NotMapped]
   public ICollection<Role> Roles { get; set; } = new List<Role>();
   public string FullName => Profiles == null ? "" : Profiles.Name;
}

public class Role : IdentityRole, IAggregateRoot
{
   public string Title { get; set; } = String.Empty;
   public virtual ICollection<UserRole>? UserRoles { get; set; }

   public object GetId() => Id;
}

public class UserRole : IdentityUserRole<string>
{
   [Required]
   public virtual User? User { get; set; }
   [Required]
   public virtual Role? Role { get; set; }
}
