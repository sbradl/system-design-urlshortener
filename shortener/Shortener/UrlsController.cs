using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shortener
{
  public record ShortenerRequest(string Url);
  public record ShortenerResponse(string Url);

  [Route("api/[controller]")]
  [ApiController]
  public class UrlsController : ControllerBase
  {
    [HttpPost]
    public ActionResult<ShortenerResponse> Shorten(ShortenerRequest request)
    {
      return Ok(new ShortenerResponse("shorturl"));
    }
  }
}
