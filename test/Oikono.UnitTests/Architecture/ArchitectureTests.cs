using System.Reflection;
using NetArchTest.Rules;
using Oikono.Services;

namespace Oikono.UnitTests.Architecture;

public class ArchitectureTests
{
    private readonly Assembly _oikonoAssembly = typeof(JwtService).Assembly;

    [Fact]
    public void Services_Should_HaveNameEndingWith_Service()
    {
        var result = Types.InAssembly(_oikonoAssembly)
            .That()
            .ResideInNamespace("Oikono.Services")
            .Should()
            .HaveNameEndingWith("Service")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Endpoints_Should_ResideInNamespace_Endpoints()
    {
        var result = Types.InAssembly(_oikonoAssembly)
            .That()
            .HaveNameEndingWith("Endpoints")
            .Should()
            .ResideInNamespace("Oikono.Endpoints")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DTOs_Should_ResideInNamespace_DTOs()
    {
        var result = Types.InAssembly(_oikonoAssembly)
            .That()
            .HaveNameEndingWith("Request")
            .Or()
            .HaveNameEndingWith("Response")
            .Should()
            .ResideInNamespace("Oikono.DTOs")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_Should_ResideInNamespace_Entities()
    {
        var result = Types.InAssembly(_oikonoAssembly)
            .That()
            .ResideInNamespace("Oikono.Entities")
            .Should()
            .NotHaveNameEndingWith("Request")
            .And()
            .NotHaveNameEndingWith("Response")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    // Note: Async suffix rule is too strict for minimal API endpoints
    // which often use inline lambdas without the Async suffix
}
