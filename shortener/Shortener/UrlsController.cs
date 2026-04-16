using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Shortener;

public record ShortenerRequest(string Url);
public record ShortenerResponse(string Url);

[Route("api/[controller]")]
[ApiController]
public class UrlsController(IConfiguration configuration, ShortenerService service) : ControllerBase
{
  [HttpPost]
  public async Task<ActionResult<ShortenerResponse>> Shorten(ShortenerRequest request)
  {
    var url = request.Url;

    if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
      return BadRequest();

    if (uri.Scheme != Uri.UriSchemeHttps)
      return BadRequest();

    var nextCode = await service.Generate();

    return Ok(new ShortenerResponse(configuration["ShortenedUrlBase"] + "/" + nextCode));
  }
}
