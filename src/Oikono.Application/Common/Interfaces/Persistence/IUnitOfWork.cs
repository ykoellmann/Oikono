using System.Data.Common;

namespace Oikono.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    /// <summary>
    /// Startet eine neue Transaktion und gibt ein Transaktionsobjekt zurück
    /// </summary>
    /// <param name="cancellationToken">Abbruchtoken für asynchrone Operationen</param>
    /// <returns>Eine Transaktion, die verwaltet werden kann</returns>
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Speichert alle Änderungen, die im aktuellen Unit of Work vorgenommen wurden
    /// </summary>
    /// <param name="cancellationToken">Abbruchtoken für asynchrone Operationen</param>
    /// <returns>Task zur Steuerung der Asynchronität</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
