# Ticket Buddy
A simple ticket booking platform for events built with a distributed architecture designed to run locally.

Key technologies/choices:
- ASP.NET Core
- Docker
- RabbitMQ
- MassTransit
- OpenTelemetry
- .NET Aspire
- Transactional Outbox Pattern

The aim is to demonstrate a fully observable and decoupled system.

## Observability

Ticket Buddy uses OpenTelemetry to provide comprehensive observability across all services. 
The distributed telemetry data is visualized in the Aspire dashboard.

![Observability Architecture](./Observability.png)
