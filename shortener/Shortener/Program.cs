using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shortener.Postgres;
using Shortener.Redis;

namespace Shortener;

public static class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();

    builder.Services.AddControllers();
    builder.Services.AddRedis("idstore-redis");
    builder.Services.AddSingleton<UniqueIdService, RedisUniqueIdService>();
    builder.Services.AddSingleton<ShortenerService, ShortenerServiceImpl>();
    builder.AddNpgsqlDataSource("urlshortener");
    builder.Services.AddSingleton<UrlStore, PostgresUrlStore>();

    var app = builder.Build();
    app.MapControllers();
    // app.UseHttpsRedirection();

    app.Run();
  }
}
