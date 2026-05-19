using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Shortener.Redis;

public static class RedisServiceCollectionExtensions
{
  public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
  {
    services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
      var config = sp.GetRequiredService<IConfiguration>().GetConnectionString(connectionString);

      return ConnectionMultiplexer.Connect(config!);
    });

    return services;
  }
}
