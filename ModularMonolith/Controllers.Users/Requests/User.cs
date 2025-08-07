using Domain.Users.Primitives;

namespace Controllers.Users.Requests;

public record UserPayload(Name FullName, Email Email);