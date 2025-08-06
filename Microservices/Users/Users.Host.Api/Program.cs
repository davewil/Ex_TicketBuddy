using Api.Hosting;
using Shared.Hosting;

var configuration = Configuration.Build();
var builder = WebApplication.CreateBuilder(args);
await new UserApi(builder, configuration).RunAsync();