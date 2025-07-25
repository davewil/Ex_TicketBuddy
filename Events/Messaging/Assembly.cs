using System.Reflection;

namespace Messaging;

public static class MessagingAssembly
{
    public static Assembly Assembly => typeof(MessagingAssembly).Assembly;
}