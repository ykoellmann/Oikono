namespace Oikono.Application.Common.Interfaces.Persistence;

public interface ITransaction : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Führt alle in der Transaktion durchgeführten Operationen aus
    /// </summary>
    /// <param name="cancellationToken">Abbruchtoken für asynchrone Operationen</param>
    /// <returns>Task zur Steuerung der Asynchronität</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Macht alle in der Transaktion durchgeführten Operationen rückgängig
    /// </summary>
    /// <param name="cancellationToken">Abbruchtoken für asynchrone Operationen</param>
    /// <returns>Task zur Steuerung der Asynchronität</returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
