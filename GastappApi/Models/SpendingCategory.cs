using System;
using System.Collections.Generic;

namespace GastappApi.Models;

public partial class SpendingCategory
{
    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string CategoryName { get; set; } = null!;
    public bool IsInitial { get; set; } = false;

    public virtual ICollection<Spending> Spendings { get; set; } = new List<Spending>();

    public virtual User User { get; set; } = null!;
}
