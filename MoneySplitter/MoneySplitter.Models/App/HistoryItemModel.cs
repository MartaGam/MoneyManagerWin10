using System;

namespace MoneySplitter.Models.App
{
	public class HistoryItemModel
	{
		public string Title { get; set; }
		public double Cost { get; set; }
		public DateTime Date { get; set; }
		public bool IsPositive => Cost > 0;
	}
}
