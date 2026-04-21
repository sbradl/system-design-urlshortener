using System.Threading.Tasks;

namespace Redirector;

public interface UrlStore
{
  Task<string?> Get(string code);
}
