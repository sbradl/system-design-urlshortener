using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Shortener.Postgres;

public static class PostgresServiceCollectionExtensions
{
  public static IServiceCollection AddPostgres(this IServiceCollection services, string connectionString)
  {
    services.AddSingleton(NpgsqlDataSource.Create(connectionString));

    return services;
  }
}
