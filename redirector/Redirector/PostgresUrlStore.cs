using System.Threading.Tasks;
using Npgsql;

namespace Redirector;

public class PostgresUrlStore(NpgsqlDataSource dataSource) : UrlStore
{
    public async Task<string?> Get(string code)
    {
        await using var cmd = dataSource.CreateCommand("SELECT source_url FROM urls WHERE short_code = $1");
        cmd.Parameters.AddWithValue(code);
        return (string?)await cmd.ExecuteScalarAsync();
    }
}