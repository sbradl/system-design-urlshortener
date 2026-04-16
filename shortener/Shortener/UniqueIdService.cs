using System.Threading.Tasks;

namespace Shortener;

public interface UniqueIdService
{
  Task<ulong> NextId();
}
