using System;
using System.Collections.Generic;

namespace BazaarResearchApp.Models;

public partial class Search
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int NumberOfListing { get; set; }

    public int? SinglePriceAverage { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual ICollection<List> Lists { get; set; } = new List<List>();
}
