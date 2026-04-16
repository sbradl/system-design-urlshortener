using System.Threading.Tasks;

namespace Shortener;

public interface ShortenerService
{
  Task<string> Generate();
}
