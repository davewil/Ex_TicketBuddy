using Application.Users;
using Domain.Users.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Users.Persistence;

namespace Infrastructure.Users.Configuration;

public static class Services
{
    public static void ConfigureUsersServices(this IServiceCollection services)
    {
        services.AddScoped<IAmAUserRepository, UserRepository>();
        services.AddScoped<UserService>();
    }
}