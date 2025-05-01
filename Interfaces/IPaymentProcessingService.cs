namespace ConcertTicketApp.Interfaces
{
	public interface IPaymentProcessingService
	{
		public Task<PaymentProcessingSerivceResult> ProcessPaymentAsync(string paymentToken);
	}

	public class PaymentProcessingSerivceResult
	{
		public bool IsSuccess { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public string PurchaseReceipt { get; set; } = string.Empty;
	}


}
