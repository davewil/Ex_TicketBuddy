using Domain.Events.Primitives;

namespace Controllers.Events.Requests;

public record EventPayload(Name Name, DateTimeOffset Date);