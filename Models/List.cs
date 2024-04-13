using System;
using System.Collections.Generic;

namespace BazaarResearchApp.Models;

public partial class List
{
    public int Id { get; set; }

    public int SearchId { get; set; }

    public int Price { get; set; }

    public int Quantity { get; set; }

    public int SinglePrice { get; set; }

    public string? Seller { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public virtual Search Search { get; set; } = null!;
}
public class ListsViewModel
{
    public List<List> Lists { get; set; } = null!;
    public string ItemName { get; set; } = null!;
    public string Date { get; set; } = null!;
}