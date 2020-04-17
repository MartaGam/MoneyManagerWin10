namespace MoneyManager.Models
{
	public class NotificationUserModel
	{
		public int Id { get; set; }
		public int OwnerId { get; set; }
		public string Text { get; set; }
		public int TransactionId { get; set; }
	}
}