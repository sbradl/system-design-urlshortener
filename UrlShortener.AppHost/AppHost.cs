var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter(
    name: "postgres-user",
    value: "urlshortener",
    secret: false);
var postgresPassword = builder.AddParameter("postgres-password", "urlshortener");


var urlstore_pg = builder.AddPostgres("urlstore-pg", userName: postgresUser, password: postgresPassword)
    .WithImage("postgres", "18-alpine")
    .WithBindMount("../data/urlstore_pg", "/var/lib/postgresql")
    .WithBindMount("../db/init.sql", "/docker-entrypoint-initdb.d/init.sql", isReadOnly: true);

var urlshortener = urlstore_pg.AddDatabase("urlshortener");

var idstore_redis = builder.AddRedis("idstore-redis")
    .WithImage("redis", "8.6.2-alpine")
    .WithBindMount("../data/idstore_redis", "/data")
    .WithArgs("--appendonly", "yes");

var redirector = builder.AddProject<Projects.Redirector>("redirector")
    .WithHttpEndpoint()
    .WithReference(urlshortener)
    .WaitFor(urlstore_pg);

var redirectorUri = redirector.GetEndpoint("http");

var shortener = builder.AddProject<Projects.Shortener>("shortener")
    .WithHttpEndpoint()
    .WithEnvironment("SHORTENED_URL_BASE", redirectorUri)
    .WithReference(urlshortener)
    .WithReference(idstore_redis)
    .WaitFor(urlstore_pg)
    .WaitFor(idstore_redis);

var shortenerUrl = shortener.GetEndpoint("http");

var frontend = builder.AddProject<Projects.FrontendHost>("frontend")
    .WithHttpEndpoint(targetPort: 8080)
    .WithReference(shortener)
    .WaitFor(shortener);

builder.Build().Run();
