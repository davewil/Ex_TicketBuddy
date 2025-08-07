using Domain.Events.Primitives;

namespace Controllers.Events.Requests;

public record EventPayload(EventName Name);