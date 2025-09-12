using Oikono.Domain.Assets;
using Oikono.Domain.Assets.ValueObjects;

namespace Oikono.Application.Common.Interfaces.Persistence;

public interface IAssetRepository :  IRepository<Asset, AssetId>
{
    
}