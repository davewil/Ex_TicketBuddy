using System.Reflection;

namespace Domain.Events.Messaging;

public static class EventsDomainMessaging
{
    public static Assembly Assembly => typeof(EventsDomainMessaging).Assembly;
}