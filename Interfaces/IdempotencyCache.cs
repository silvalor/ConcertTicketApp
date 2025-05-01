namespace ConcertTicketApp.Interfaces
{
	public interface IIdempotencyCache
	{
		Task<T> AddAndReturn<T>(string key, T value, DateTime expiry) where T : class;
		Task<IdempotencyCacheResponse<T>> TryGet<T>(string key) where T : class;
	}

	public class IdempotencyCacheResponse<T> where T : class
	{
		public T Response { get; set; }
		public bool IsSuccess { get; set; }

	}
}
