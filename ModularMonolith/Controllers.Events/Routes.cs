namespace Controllers.Events;

public static class Routes
{
    public const string Event = "event";
    public const string TheEvent = $"{Event}/{{id:guid}}";
}