var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sqlserver")
    .WithPassword(builder.AddParameter("Password", "YourStrong@Passw0rd"))
    .WithDataVolume("TicketBuddy.SqlServer")
    .WithHostPort(1450);

var sqlHostAddress = sqlServer.Resource.PrimaryEndpoint.Property(EndpointProperty.Host);
var sqlHostPort = sqlServer.Resource.PrimaryEndpoint.Property(EndpointProperty.Port);

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithImage("masstransit/rabbitmq")
    .WithDataVolume("TicketBuddy.RabbitMQ");

var rabbitHostAddress = rabbitmq.Resource.PrimaryEndpoint.Property(EndpointProperty.Host);
var rabbitHostPort = rabbitmq.Resource.PrimaryEndpoint.Property(EndpointProperty.Port);

var eventsMigrations = builder.AddProject<Projects.Events_Host_Migrations>("events-migrations")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithEnvironment("ENVIRONMENT", "Development");

var eventsApi = builder.AddProject<Projects.Events_Host_Api>("events-api")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(eventsMigrations)
    .WaitFor(eventsMigrations)
    .WithEnvironment("ENVIRONMENT", "Development");

builder.AddProject<Projects.Events_Host_Messaging_Outbox>("events-messaging-outbox")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(eventsApi)
    .WaitFor(eventsApi)
    .WithEnvironment("ENVIRONMENT", "Development");

var ticketsMigrations = builder.AddProject<Projects.Tickets_Host_Migrations>("tickets-migrations")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithEnvironment("ENVIRONMENT", "Development");

builder.AddProject<Projects.Tickets_Host_Messaging_Inbox>("tickets-messaging-inbox")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(ticketsMigrations)
    .WaitFor(ticketsMigrations)
    .WithEnvironment("ENVIRONMENT", "Development");

var usersMigrations = builder.AddProject<Projects.Users_Host_Migrations>("users-migrations")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithEnvironment("ENVIRONMENT", "Development");

var usersApi = builder.AddProject<Projects.Users_Host_Api>("users-api")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(usersMigrations)
    .WaitFor(usersMigrations)
    .WithHttpEndpoint(port: 8082, targetPort: 8080)
    .WithEnvironment("ENVIRONMENT", "Development");

builder.AddProject<Projects.Users_Host_Messaging_Outbox>("users-messaging-outbox")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(usersApi)
    .WaitFor(usersApi)
    .WithEnvironment("ENVIRONMENT", "Development");

var app = builder.Build();
await app.RunAsync();
