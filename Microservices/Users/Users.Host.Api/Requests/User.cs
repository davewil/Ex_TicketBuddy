using Users.Domain.Primitives;

namespace Api.Requests;

public record UserPayload(Name FullName, Email Email);