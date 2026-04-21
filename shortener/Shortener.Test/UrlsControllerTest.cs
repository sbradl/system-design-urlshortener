using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shortener.Test;

[TestClass]
public class UrlsControllerTest
{
  private readonly ShortenerService service = new ShortenerServiceDummy();
  private readonly UrlStoreMock urlStore = new UrlStoreMock();
  private readonly UrlsController controller;

  public UrlsControllerTest()
  {
    var kv = new Dictionary<string, string?>()
    {
      { "ShortenedUrlBase", "https://shortener.com"}
    };

    var configuration = new ConfigurationBuilder()
      .AddInMemoryCollection(kv)
      .Build();


    controller = new UrlsController(configuration, service, urlStore);
  }

  [TestMethod]
  public async Task GivenInvalidRequest_ReturnsBadRequest()
  {
    await AssertBadRequest(new ShortenerRequest(string.Empty));
    await AssertBadRequest(new ShortenerRequest("   "));
    await AssertBadRequest(new ShortenerRequest("no-url"));
  }

  [TestMethod]
  public async Task GivenNonHttpsUrl_ReturnsBadRequest()
  {
    await AssertBadRequest(new ShortenerRequest("http://test.com"));
    await AssertBadRequest(new ShortenerRequest("ftp://test.com"));
    await AssertBadRequest(new ShortenerRequest("file:///etc/passwd"));
  }

  [TestMethod]
  public async Task GivenValidHttpsUrl_ReturnsOk()
  {
    var result = await Shorten("https://test.com");
    Assert.IsNotNull(result.Url);
  }

  [TestMethod]
  public async Task Shorten_ReturnsShortUrl()
  {
    var result = await Shorten("https://test.com");
    Assert.StartsWith("https://shortener.com/", result.Url);
  }

  [TestMethod]
  public async Task ShorteningSameUrl_ReturnsDifferentShortUrls()
  {
    var result1 = await Shorten("https://test.com");
    var result2 = await Shorten("https://test.com");

    Assert.AreNotEqual(result1.Url, result2.Url);
  }

  [TestMethod]
  public async Task ShortenedUrl_IsSaved()
  {
    var result = await Shorten("https://test.com");
    string pathAndQuery = new Uri(result.Url).PathAndQuery.TrimStart('/');

    Assert.AreEqual("https://test.com", urlStore.GetSourceUrlFor(pathAndQuery));
  }

  private async Task AssertBadRequest(ShortenerRequest request)
  {
    var result = await Shorten(request);

    Assert.IsInstanceOfType<BadRequestResult>(result.Result);
  }

  private async Task<ShortenerResponse> Shorten(string url)
  {
    var result = Assert.IsInstanceOfType<OkObjectResult>(
      (await controller.Shorten(new ShortenerRequest(url)))
        .Result);

    return Assert.IsInstanceOfType<ShortenerResponse>(result.Value);
  }

  private async Task<ActionResult<ShortenerResponse>> Shorten(ShortenerRequest request)
  {
    return await controller.Shorten(request);
  }

  private sealed class ShortenerServiceDummy : ShortenerService
  {
    private int id;

    public Task<string> Generate()
    {
      return Task.FromResult(id++.ToString());
    }
  }

  private sealed class UrlStoreMock : UrlStore
  {
    private readonly Dictionary<string, string> urls = [];

    public Task Save(string sourceUrl, string shortUrlPath)
    {
      this.urls.Add(shortUrlPath, sourceUrl);

      return Task.CompletedTask;
    }

    public string GetSourceUrlFor(string urlPath)
    {
      return this.urls[urlPath];
    }
  }
}
