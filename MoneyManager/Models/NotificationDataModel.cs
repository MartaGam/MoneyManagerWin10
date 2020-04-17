namespace MoneyManager.Models
{
	public class NotificationDataModel
	{
		public int Id { get; set; }
		public int OwnerId { get; set; }
		public int TargetUserId { get; set; }
		public string Text { get; set; }
		public int TransactionId { get; set; }
	}
}