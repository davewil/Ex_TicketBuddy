namespace Controllers.Tickets;

public static class Routes
{
    public const string Events = "events";
    public const string Tickets = $"{Events}/{{id:guid}}/tickets";
}