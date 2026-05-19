using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;
namespace FrontendHost;

public static class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    var shortenerApiUrl = builder.Configuration["services:shortener:https:0"]
              ?? builder.Configuration["services:shortener:http:0"]
              ?? throw new Exception("shortener service api url not available");

    builder.Services
      .AddReverseProxy()
      .LoadFromMemory([
          new RouteConfig
          {
            RouteId = "shortener-api-route",
            ClusterId = "shortener-api-cluster",
            Match = new()
            {
              Path = "/api/{**catch-all}"
            },
          }
        ], [
        new ClusterConfig
        {
          ClusterId = "shortener-api-cluster",
          Destinations = new Dictionary<string, DestinationConfig>
          {
            ["shortener"] = new() { Address = shortenerApiUrl }
          }
        }
      ]);

    var app = builder.Build();

    app.MapReverseProxy();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");

    app.Run();
  }
}
