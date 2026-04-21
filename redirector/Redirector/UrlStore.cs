namespace Redirector;

public interface UrlStore
{
  Task<string?> Get(string code);
}
