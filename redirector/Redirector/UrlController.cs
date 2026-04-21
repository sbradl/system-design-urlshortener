using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Redirector;

[ApiController]
public class UrlController(UrlStore urlStore) : ControllerBase
{
  [HttpGet("{**pathAndQuery}")]
  public async Task<IActionResult> GetSourceUrl(string pathAndQuery)
  {
    var sourceUrl = await urlStore.Get(pathAndQuery);

    if (sourceUrl == null)
      return NotFound();

    return RedirectPermanent(sourceUrl);
  }
}
