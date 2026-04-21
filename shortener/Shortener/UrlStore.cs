using System.Threading.Tasks;

namespace Shortener;

public interface UrlStore
{
  Task Save(string sourceUrl, string shortUrlPath);
}
