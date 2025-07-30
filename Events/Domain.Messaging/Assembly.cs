using System.Reflection;

namespace Events.Domain.Messaging;

public static class EventsDomainMessaging
{
    public static Assembly Assembly => typeof(EventsDomainMessaging).Assembly;
}