using System;
using System.Collections.Generic;

namespace GastappApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string PhoneNumber { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public virtual ICollection<SpendingCategory> SpendingCategories { get; set; } = new List<SpendingCategory>();
}
