using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace Shortener.Redis;

public sealed class RedisUniqueIdService(IConnectionMultiplexer redis) : UniqueIdService
{
  private const long RangeSize = 1000;
  private const string CounterKey = "url_id_counter";

  private readonly IDatabase db = redis.GetDatabase();
  private readonly SemaphoreSlim refillLock = new(1, 1);
  private long current;
  private long rangeEnd = -1;

  public async Task<ulong> NextId()
  {
    await refillLock.WaitAsync();
    try
    {
      if (current > rangeEnd)
      {
        var end = await db.StringIncrementAsync(CounterKey, RangeSize);
        rangeEnd = end;
        current = end - RangeSize + 1;
      }
      return (ulong)current++;
    }
    finally
    {
      refillLock.Release();
    }
  }
}
