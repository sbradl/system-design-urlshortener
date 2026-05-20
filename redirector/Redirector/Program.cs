using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Redirector;

public static class Program
{
  public static void Main(string[] args)
  {

    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();

    builder.AddNpgsqlDataSource("urlshortener");
    builder.Services.AddSingleton<UrlStore, PostgresUrlStore>();
    builder.Services.AddControllers();

    var app = builder.Build();

    app.MapControllers();

    app.Run();
  }
}
