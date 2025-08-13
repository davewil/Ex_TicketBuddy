using Domain.Events.Primitives;

namespace Controllers.Events.Requests;

public record EventPayload(EventName EventName, DateTimeOffset Date, Venue Venue);