using System.Reflection;
using Oikono.Api.Common.Controllers;
using Oikono.UnitTests.Rules;
using NetArchTest.Rules;

namespace Oikono.Api.UnitTests.Architecture;

public class ArchitectureTests
{
    private readonly Assembly _apiAssembly = typeof(DependencyInjection).Assembly;

    [Fact]
    public void AsyncMethods_Should_HaveSuffix_Async()
    {
        var result = Types.InAssembly(_apiAssembly)
            .That()
            .DoNotInherit(typeof(ApiController))
            .Should()
            .MeetCustomRule(new AsyncMethodsHaveSuffixAsyncRule())
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}