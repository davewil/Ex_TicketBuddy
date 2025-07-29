using System.Reflection;

namespace Domain.Messaging;

public static class DomainMessagingAssembly
{
    public static Assembly Assembly => typeof(DomainMessagingAssembly).Assembly;
}