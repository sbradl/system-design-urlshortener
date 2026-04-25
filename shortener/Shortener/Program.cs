using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shortener.Postgres;
using Shortener.Redis;

namespace Shortener;

public static class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddCors(options =>
    {
      options.AddDefaultPolicy(
      policy => policy
        .WithOrigins("http://localhost")
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());
    });
    builder.Services.AddRedis(builder.Configuration["RedisIdStoreConnectionString"]!);
    builder.Services.AddSingleton<UniqueIdService, RedisUniqueIdService>();
    builder.Services.AddSingleton<ShortenerService, ShortenerServiceImpl>();
    builder.Services.AddPostgres(builder.Configuration["PostgresUrlStoreConnectionString"]!);
    builder.Services.AddSingleton<UrlStore, PostgresUrlStore>();

    var app = builder.Build();
    app.UseCors();
    app.MapControllers();
    // app.UseHttpsRedirection();

    app.Run();
  }
}
