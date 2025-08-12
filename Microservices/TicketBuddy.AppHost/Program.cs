using Common.Environment;

const string Environment = "Environment";
var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("SqlServerMicroservices")
    .WithPassword(builder.AddParameter("Password", "YourStrong@Passw0rd"))
    .WithDataVolume("TicketBuddy.SqlServer")
    .WithHostPort(1450)
    .WithLifetime(ContainerLifetime.Persistent);

var eventDb = sqlServer.AddDatabase("TicketBuddyEvents");
var ticketDb = sqlServer.AddDatabase("TicketBuddyTickets");
var userDb = sqlServer.AddDatabase("TicketBuddyUsers");

var rabbitmq = builder
    .AddRabbitMQ("Messaging")
    .WithImage("masstransit/rabbitmq")
    .WithDataVolume("TicketBuddy.RabbitMQ")
    .WithHttpEndpoint(port: 5672, targetPort: 5672)
    .WithHttpsEndpoint(port: 15672, targetPort: 15672);

var eventsMigrations = builder.AddProject<Projects.Events_Host_Migrations>("events-migrations")
    .WithReference(eventDb)
    .WaitFor(eventDb)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var eventsApi = builder.AddProject<Projects.Events_Host_Api>("events-api")
    .WithReference(eventDb)
    .WaitFor(eventDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(eventsMigrations)
    .WaitFor(eventsMigrations)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var eventsMessagingOutbox = builder.AddProject<Projects.Events_Host_Messaging_Outbox>("events-messaging-outbox")
    .WithReference(eventDb)
    .WaitFor(eventDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(eventsApi)
    .WaitFor(eventsApi)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var usersMigrations = builder.AddProject<Projects.Users_Host_Migrations>("users-migrations")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var usersApi = builder.AddProject<Projects.Users_Host_Api>("users-api")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(usersMigrations)
    .WaitFor(usersMigrations)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var usersMessagingOutbox = builder.AddProject<Projects.Users_Host_Messaging_Outbox>("users-messaging-outbox")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(usersApi)
    .WaitFor(usersApi)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var ticketsMigrations = builder.AddProject<Projects.Tickets_Host_Migrations>("tickets-migrations")
    .WithReference(ticketDb)
    .WaitFor(ticketDb)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

builder.AddProject<Projects.Tickets_Host_Messaging_Inbox>("tickets-messaging-inbox")
    .WithReference(ticketDb)
    .WaitFor(ticketDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(ticketsMigrations)
    .WaitFor(ticketsMigrations)
    .WaitFor(eventsMessagingOutbox)
    .WaitFor(usersMessagingOutbox)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var apiGateway = builder.AddProject<Projects.TicketBuddy_ApiGateway>("api-gateway")
    .WithReference(eventsApi)
    .WaitFor(eventsApi)
    .WithReference(usersApi)
    .WaitFor(usersApi)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

var dataSeeder = builder.AddProject<Projects.TicketBuddy_DataSeeder>("DataSeeder")
    .WithReference(apiGateway)
    .WaitFor(apiGateway)
    .WithEnvironment(Environment, CommonEnvironment.LocalDevelopment.ToString);

builder.AddViteApp(name: "User-Interface", workingDirectory: "../../UI")
    .WithReference(apiGateway)
    .WaitFor(apiGateway)
    .WithReference(dataSeeder)
    .WaitFor(dataSeeder)
    .WithNpmPackageInstallation();

var app = builder.Build();
await app.RunAsync();
