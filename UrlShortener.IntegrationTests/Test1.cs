using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Aspire.Hosting.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UrlShortener.IntegrationTests;

[TestClass]
public sealed class Test1
{
  private HttpClient GetClient()
  {
    return AspireTestHost.App.CreateHttpClient("shortener");
  }

  [TestMethod]
  public async Task CreateUrl_ShouldReturnSuccess()
  {
    var client = GetClient();

    var response = await client.PostAsJsonAsync("/api/urls", new
    {
      url = "https://example.com"
    });

    Assert.IsTrue(response.IsSuccessStatusCode);
  }
}
