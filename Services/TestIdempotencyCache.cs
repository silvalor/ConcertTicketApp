using ConcertTicketApp.Interfaces;

namespace ConcertTicketApp.Services
{
	public class TestIdempotencyCache : IIdempotencyCache
	{
		private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

		public async Task Add<T>(string key, T value, DateTime expiry) where T : class
		{
			_cache.Add(key, value);
		}

		public async Task<T> AddAndReturn<T>(string key, T value, DateTime expiry) where T : class
		{
			_cache.Add(key, value);
			return value;
		}

		public async Task<IdempotencyCacheResponse<T>> TryGet<T>(string key) where T : class
		{
			if (_cache.ContainsKey(key))
			{
				return new IdempotencyCacheResponse<T>
				{
					Response = _cache[key] as T,
					IsSuccess = true
				};
			}
			else
			{
				return new IdempotencyCacheResponse<T>
				{
					Response = null,
					IsSuccess = false
				};
			}
		}
	}
}
