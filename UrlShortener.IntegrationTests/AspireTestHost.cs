using System.Threading.Tasks;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UrlShortener.IntegrationTests;

[TestClass]
public static class AspireTestHost
{
  public static DistributedApplication App { get; private set; } = default!;

  [AssemblyInitialize]
  public static async Task Start(TestContext context)
  {
    var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.UrlShortener_AppHost>();

    App = await builder.BuildAsync();

    await App.StartAsync();
  }

  [AssemblyCleanup]
  public static async Task Stop()
  {
    await App.StopAsync();
  }
}
