using System.Threading.Tasks;
using Npgsql;

namespace Shortener.Postgres;

public class PostgresUrlStore(NpgsqlDataSource dataSource) : UrlStore
{
  public async Task Save(string sourceUrl, string shortUrlPath)
  {
    await using var cmd = dataSource.CreateCommand("INSERT INTO urls (short_code, source_url) VALUES ($1, $2)");
    cmd.Parameters.AddWithValue(shortUrlPath);
    cmd.Parameters.AddWithValue(sourceUrl);
    await cmd.ExecuteNonQueryAsync();
  }
}
