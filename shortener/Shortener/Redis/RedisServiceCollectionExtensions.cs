using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Shortener.Redis;

public static class RedisServiceCollectionExtensions
{
  public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
  {
    services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

    return services;
  }
}
