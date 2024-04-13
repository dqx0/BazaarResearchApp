using System;
using System.Collections.Generic;

namespace BazaarResearchApp.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int MaterialId { get; set; }
    public int Quantity { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Item Material { get; set; } = null!;
}
