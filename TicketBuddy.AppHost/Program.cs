var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sqlserver")
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
    .WithDataVolume("TicketBuddy.RabbitMQ");

var eventsMigrations = builder.AddProject<Projects.Events_Host_Migrations>("events-migrations")
    .WithReference(eventDb)
    .WaitFor(eventDb)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var eventsApi = builder.AddProject<Projects.Events_Host_Api>("events-api")
    .WithReference(eventDb)
    .WaitFor(eventDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(eventsMigrations)
    .WaitFor(eventsMigrations)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var eventsMessagingOutbox = builder.AddProject<Projects.Events_Host_Messaging_Outbox>("events-messaging-outbox")
    .WithReference(eventDb)
    .WaitFor(eventDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(eventsApi)
    .WaitFor(eventsApi)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var usersMigrations = builder.AddProject<Projects.Users_Host_Migrations>("users-migrations")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var usersApi = builder.AddProject<Projects.Users_Host_Api>("users-api")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(usersMigrations)
    .WaitFor(usersMigrations)
    .WithHttpEndpoint(port: 8082, targetPort: 8080)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var usersMessagingOutbox = builder.AddProject<Projects.Users_Host_Messaging_Outbox>("users-messaging-outbox")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(usersApi)
    .WaitFor(usersApi)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var ticketsMigrations = builder.AddProject<Projects.Tickets_Host_Migrations>("tickets-migrations")
    .WithReference(ticketDb)
    .WaitFor(ticketDb)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

builder.AddProject<Projects.Tickets_Host_Messaging_Inbox>("tickets-messaging-inbox")
    .WithReference(ticketDb)
    .WaitFor(ticketDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(ticketsMigrations)
    .WaitFor(ticketsMigrations)
    .WaitFor(eventsMessagingOutbox)
    .WaitFor(usersMessagingOutbox)
    .WithEnvironment("ENVIRONMENT", "LocalDevelopment");

var app = builder.Build();
await app.RunAsync();
