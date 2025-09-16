using Oikono.Domain.Assets.ValueObjects;
using Oikono.Domain.Models;

namespace Oikono.Domain.Assets;

public class Asset : AggregateRoot<AssetId>
{
    public Asset(string fileName, string contentType, byte[] data)
    {
        FileName = fileName;
        ContentType = contentType;
        Data = data;
    }

    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public byte[] Data { get; private set; } = [];
}