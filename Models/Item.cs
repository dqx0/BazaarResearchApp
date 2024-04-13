using System;
using System.Collections.Generic;

namespace BazaarResearchApp.Models;

public partial class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int NpcPrice { get; set; }

    public virtual ICollection<Recipe> RecipeItems { get; set; } = new List<Recipe>();

    public virtual ICollection<Recipe> RecipeMaterials { get; set; } = new List<Recipe>();

    public virtual ICollection<Search> Searches { get; set; } = new List<Search>();
}
