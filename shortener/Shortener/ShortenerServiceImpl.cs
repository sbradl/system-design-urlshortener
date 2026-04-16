using System.Threading.Tasks;

namespace Shortener;

public class ShortenerServiceImpl(UniqueIdService idService) : ShortenerService
{
  public async Task<string> Generate()
  {
    var id = await idService.NextId();

    return ToBase62(id);
  }

  private static string ToBase62(ulong n)
  {
    const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    if (n == 0)
      return "0";

    var s = string.Empty;
    while (n > 0)
    {
      s = chars[(int)(n % 62)] + s;
      n /= 62;
    }

    return s;
  }
}
