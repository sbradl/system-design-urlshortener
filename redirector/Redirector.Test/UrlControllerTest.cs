using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Redirector.Test;

[TestClass]
public class UrlControllerTest
{
  private readonly UrlStoreMock urlStore = new();
  private readonly UrlController controller;

  public UrlControllerTest()
  {
    controller = new UrlController(urlStore);
  }

  [TestMethod]
  public async Task GivenUnknownPath_ReturnsNotFound()
  {
    var result = await controller.GetSourceUrl("unknown");

    Assert.IsInstanceOfType<NotFoundResult>(result);
  }

  [TestMethod]
  public async Task GivenKnownPath_ReturnsRedirect()
  {
    urlStore.Save("https://original.com", "abc123");

    var result = await controller.GetSourceUrl("abc123");

    var redirect = Assert.IsInstanceOfType<RedirectResult>(result);
    Assert.AreEqual("https://original.com", redirect.Url);
  }

  private sealed class UrlStoreMock : UrlStore
  {
    private readonly Dictionary<string, string> urls = [];

    public void Save(string sourceUrl, string shortUrlPath)
    {
      urls.Add(shortUrlPath, sourceUrl);
    }

    public Task<string?> Get(string shortUrlPath)
    {
      urls.TryGetValue(shortUrlPath, out var sourceUrl);

      return Task.FromResult(sourceUrl);
    }
  }
}
