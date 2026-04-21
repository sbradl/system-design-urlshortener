using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Redirector;

public static class Program
{
  public static void Main(string[] args)
  {

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddSingleton(NpgsqlDataSource.Create(builder.Configuration["UrlStoreConnectionString"]!));
    builder.Services.AddSingleton<UrlStore, PostgresUrlStore>();

    var app = builder.Build();

    app.MapControllers();

    app.Run();
  }
}
