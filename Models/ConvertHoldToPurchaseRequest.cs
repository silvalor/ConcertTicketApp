namespace ConcertTicketApp.Models
{
	public class ConvertHoldToPurchaseRequest
	{
		public string PaymentToken { get; set; }
		public string Operation { get; set; } = "convert-to-purchase";
	}
}
