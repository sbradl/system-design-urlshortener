using System.Threading;
using System.Threading.Tasks;

namespace Shortener;

public sealed class UniqueIdServiceImpl : UniqueIdService
{
  private ulong id;

  public Task<ulong> NextId()
  {
    return Task.FromResult(Interlocked.Increment(ref id));
  }
}

