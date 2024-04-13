using System;
namespace BazaarResearchApp.Models
{
	public class ItemViewModel
	{
		public Item Item { get; set; } = null!;
		public List<Search>? Searches { get; set; }
		public Profit? Profit { get; set; }
	}
	public class Profit
	{
		public Dictionary<Item, (int, int)> Recipe { get; set; } = null!;
		public int FailProfit { get; set; }
        public int Success0Profit { get; set; }
        public int Success1Profit { get; set; }
		public int Success2Profit { get; set; }
		public int Success3Profit { get; set; }
	}
}

