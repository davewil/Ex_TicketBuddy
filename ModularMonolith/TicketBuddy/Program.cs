using Common.Environment;

const string Environment = "Environment";
var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("SqlServerMonolith")
    .WithPassword(builder.AddParameter("Password", "YourStrong@Passw0rd"))
    .WithDataVolume("TicketBuddyMonolith.SqlServer")
    .WithHostPort(1450)
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("TicketBuddy");

var migrations = builder.AddProject<Projects.Host_Migrations>("Migrations")
    .WithReference(database)
    .WaitFor(database)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var api = builder.AddProject<Projects.Host_Api>("Api")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(migrations)
    .WaitFor(migrations)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var dataseeder = builder.AddProject<Projects.Host_Dataseeder>("Dataseeder")
    .WithReference(api)
    .WaitFor(api)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

builder.AddViteApp(name: "User-Interface", workingDirectory: "../../UI")
    .WithReference(api)
    .WaitFor(api)
    .WithReference(dataseeder)
    .WaitFor(dataseeder)
    .WithNpmPackageInstallation();

var app = builder.Build();
await app.RunAsync(); 
