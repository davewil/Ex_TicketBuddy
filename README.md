# Ticket Buddy
A simple ticket booking platform for events. 

Built as both a modular monolith and as microservices to compare and contrast.

## Modular Monolith

Bu

built with a distributed architecture designed to run locally.

Key technologies/choices:
- ASP.NET Core
- Docker
- RabbitMQ
- MassTransit
- OpenTelemetry
- .NET Aspire
- Transactional Outbox Pattern

The aim is to demonstrate a fully observable and decoupled system.
Admittedly, the transactional outbox pattern is overkill.

## Observability

Ticket Buddy uses OpenTelemetry to provide comprehensive observability across all services. 
The monolithic and distributed telemetry data is visualized in the Aspire dashboard.

![Observability Architecture](./Observability.png)
