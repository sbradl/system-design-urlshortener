using Microsoft.AspNetCore.Mvc;

namespace Shortener.Test;

[TestClass]
public class UrlsControllerTest
{
  [TestMethod]
  public void GivenInvalidRequest_ReturnsBadRequest()
  {
    AssertBadRequest(new ShortenerRequest(string.Empty));
    AssertBadRequest(new ShortenerRequest("   "));
    AssertBadRequest(new ShortenerRequest("no-url"));
  }

  [TestMethod]
  public void GivenHttpUrl_ReturnsBadRequest()
  {
    AssertBadRequest(new ShortenerRequest("http://test.com"));
  }

  [TestMethod]
  public void GivenValidHttpsUrl_ReturnsOk()
  {
    var result = Shorten("https://test.com");
    Assert.IsNotNull(result.Url);
  }

  private void AssertBadRequest(ShortenerRequest request)
  {
    var result = Shorten(request);

    Assert.IsInstanceOfType<BadRequestResult>(result.Result);
  }

  private ShortenerResponse Shorten(string url)
  {
    var result = Assert.IsInstanceOfType<OkObjectResult>(
        new UrlsController()
         .Shorten(new ShortenerRequest(url))
          .Result);

    return Assert.IsInstanceOfType<ShortenerResponse>(result.Value);
  }

  private ActionResult<ShortenerResponse> Shorten(ShortenerRequest request)
  {
    return new UrlsController().Shorten(request);
  }
}
