using Oikono.Domain.GitHub;

namespace Oikono.Application.Common.Interfaces.Services;

public interface IExampleHttpService
{
    Task<ExampleHttp> GetByExampleAsync(string example);
}