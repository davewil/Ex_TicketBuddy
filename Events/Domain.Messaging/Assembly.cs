using System.Reflection;

namespace Events.Domain.Messaging;

public static class DomainMessaging
{
    public static Assembly Assembly => typeof(DomainMessaging).Assembly;
}