using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

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
    builder.Services.AddSingleton<UniqueIdService, UniqueIdServiceImpl>();
    builder.Services.AddSingleton<ShortenerService, ShortenerServiceImpl>();
    builder.Services.AddSingleton(NpgsqlDataSource.Create(builder.Configuration["UrlStoreConnectionString"]!));
    builder.Services.AddSingleton<UrlStore, PostgresUrlStore>();

    var app = builder.Build();
    app.UseCors();
    app.MapControllers();
    // app.UseHttpsRedirection();

    app.Run();
  }
}
