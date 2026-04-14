using System;
using Microsoft.AspNetCore.Mvc;

namespace Shortener;

public record ShortenerRequest(string Url);
public record ShortenerResponse(string Url);

[Route("api/[controller]")]
[ApiController]
public class UrlsController : ControllerBase
{
  [HttpPost]
  public ActionResult<ShortenerResponse> Shorten(ShortenerRequest request)
  {
    var url = request.Url;

    if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
      return BadRequest();

    if (uri.Scheme != Uri.UriSchemeHttps)
      return BadRequest();

    return Ok(new ShortenerResponse("shorturl"));
  }
}
