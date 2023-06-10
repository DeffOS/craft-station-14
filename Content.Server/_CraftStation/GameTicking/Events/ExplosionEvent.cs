using Robust.Shared.Map;

namespace Content.Server.GameTicking.Events;

public sealed class ExplosionCreatedEvent : EntityEventArgs
{
    public MapCoordinates Epicenter { get; }
    public float TotalIntensity { get; }
    public float Slope { get; }
    public float MaxTileIntensity { get; }
    public float TileBreakScale { get; }
    public int MaxTileBreak { get; }
    public bool CanCreateVacuum { get; }

    public ExplosionCreatedEvent(MapCoordinates epicenter, float totalintensity, float slope, float maxTileIntensity, float tileBreakScale, int maxTileBreak, bool canCreateVacuume)
    {
        Epicenter = epicenter;
        TotalIntensity = totalintensity;
        Slope = slope;
        MaxTileIntensity = maxTileIntensity;
        TileBreakScale = tileBreakScale;
        MaxTileBreak = maxTileBreak;
        CanCreateVacuum = canCreateVacuume;
    }
}
