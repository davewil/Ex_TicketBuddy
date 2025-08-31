using Domain.Events.Primitives;

namespace Controllers.Events.Requests;

public record EventPayload(EventName EventName, DateTimeOffset StartDate, DateTimeOffset EndDate, Venue Venue, decimal Price);