var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sqlserver")
    .WithPassword(builder.AddParameter("Password", "YourStrong@Passw0rd"))
    .WithDataVolume("TicketBuddy.SqlServer")
    .WithHostPort(1450)
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("TicketBuddy");

var migrations = builder.AddProject<Projects.Host_Api>("migrations")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

builder.AddProject<Projects.Host_Migrations>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(migrations)
    .WaitFor(migrations)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var app = builder.Build();
await app.RunAsync();
