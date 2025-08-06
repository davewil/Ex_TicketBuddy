using Api.Hosting;
using Shared.Hosting;

var configuration = Configuration.Build();
var builder = WebApplication.CreateBuilder(args);
await new Api.Hosting.Api(builder, configuration).RunAsync();