using ConcertTicketApp.Interfaces;

namespace ConcertTicketApp.Services
{
	public class TestPaymentProcessor : IPaymentProcessingService
	{
		public async Task<PaymentProcessingSerivceResult> ProcessPaymentAsync(string paymentToken)
		{
			return new PaymentProcessingSerivceResult
			{
				IsSuccess = true
			};
		}
	}
}
