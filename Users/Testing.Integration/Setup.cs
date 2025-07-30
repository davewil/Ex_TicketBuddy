using Common.Environment;
using NUnit.Framework;
using Users.Domain;

namespace Integration;

[SetUpFixture]
public static class Setup
{
    [OneTimeSetUp]
    public static void BeforeAll()
    {
        CommonEnvironment.LocalTesting.SetEnvironment();
        JsonSerialization.ResetConverters();
        JsonSerialization.RegisterConverters(Converters.GetConverters);
    }
    
    [OneTimeTearDown]
    public static void AfterAll()
    {
        CommonEnvironment.LocalDevelopment.SetEnvironment();
    }
}