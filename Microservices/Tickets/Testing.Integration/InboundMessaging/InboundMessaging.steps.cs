using BDD;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Migrations;
using Persistence;
using Persistence.Events;
using Persistence.Users;
using Shared.Testing;
using Testcontainers.MsSql;
using Tickets.Integration.Messaging.Inbound;

namespace Testing.Integration.InboundMessaging;

public partial class InboundMessaging : TruncateDbSpecification
{
    private static MsSqlContainer database = null!;
    private static IServiceProvider serviceProvider = null!;
    private ITestHarness testHarness = null!;

    protected override void before_all()
    {
        database = new MsSqlBuilder().WithPortBinding(1433, true).Build();
        database.StartAsync().Await();
        database.ExecScriptAsync("CREATE DATABASE [TicketBuddy.Tickets]").GetAwaiter().GetResult();
        Migration.Upgrade(database.GetTicketBuddyConnectionString());
    }
    
    protected override void before_each()
    {
        base.before_each();
        serviceProvider = new ServiceCollection()
            .AddScoped<EventRepository>()
            .AddScoped<UserRepository>()
            .AddDbContext<EventDbContext>(x => x.UseSqlServer(database.GetTicketBuddyConnectionString()))
            .AddDbContext<UserDbContext>(x => x.UseSqlServer(database.GetTicketBuddyConnectionString()))
            .AddMassTransitTestHarness(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                var ticketsIntegrationInboundAssembly = TicketsIntegrationMessagingInbound.Assembly;
                x.AddConsumers(ticketsIntegrationInboundAssembly);

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);
        testHarness = serviceProvider.GetRequiredService<ITestHarness>();
        testHarness.Start().Await();
    }
    
    protected override void after_each()
    {
        testHarness.Stop().Await();
        base.after_each();
    }
    
    protected override void after_all()
    {
        database.DisposeAsync().GetAwaiter().GetResult();
        base.after_all();
    }
}