using Content.Server.Explosion.EntitySystems;
using Content.Server.GameTicking.Events;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.Craft.DistantExplosions
{
    public sealed class DistantExplosionsSystem : EntitySystem
    {
        [Dependency] private readonly IMapManager _mapMan = default!;
        [Dependency] private readonly IRobustRandom _rndMan = default!;
        [Dependency] private readonly SharedAudioSystem _audSys = default!;
        [Dependency] private readonly ExplosionSystem _expSys = default!;


        private readonly string _farSound = "/Audio/_CraftStation/Effects/explosionfar.ogg";
        private readonly string _distSound = "/Audio/_CraftStation/Effects/explosiondistant.ogg";
        private IReadOnlyList<string> _dprsSounds = new List<string> { "/Audio/_CraftStation/Effects/explosioncreak1.ogg", "/Audio/_CraftStation/Effects/explosioncreak2.ogg" };

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<ExplosionCreatedEvent>(OnExplosion);
        }


        private void OnExplosion(ExplosionCreatedEvent ev)
        {
            if (ev.TotalIntensity < 60) return;
            var mapCoords = ev.Epicenter;

            //TODO: Сделать работу звука на все задетые гриды большой массы (ака станции с размером)
            EntityUid? referenceGrid = null;
            float mass = 0;

            float radius = _expSys.IntensityToRadius(ev.TotalIntensity, ev.Slope, ev.MaxTileIntensity);
            var box = Box2.CenteredAround(mapCoords.Position, (radius, radius));

            foreach (var grid in _mapMan.FindGridsIntersecting(mapCoords.MapId, box))
            {
                if (TryComp(grid.Owner, out PhysicsComponent? physics) && physics.Mass > mass)
                {
                    mass = physics.Mass;
                    referenceGrid = grid.Owner;
                }
            }

            if (referenceGrid is null) return;

            var expCoors = EntityCoordinates.FromMap(_mapMan.GetMapEntityId(mapCoords.MapId), mapCoords, EntityManager);

            var all = Filter.BroadcastGrid(referenceGrid.Value);

            var expFalloff = Math.Clamp(ev.TotalIntensity / 40 * 10, 30, 500);

            _audSys.Play(_farSound, all, expCoors, true, AudioParams.Default.WithAttenuation(Attenuation.LinearDistance).WithVolume(-1f).WithMaxDistance(expFalloff / 2.5f + 4));
            _audSys.Play(_distSound, all, expCoors, true, AudioParams.Default.WithAttenuation(Attenuation.LinearDistance).WithVolume(2f).WithMaxDistance(expFalloff).WithReferenceDistance(expFalloff / 2.5f));
            if (ev.CanCreateVacuum && ev.TotalIntensity >= 200)
                _audSys.PlayGlobal(_rndMan.Pick(_dprsSounds), all, true, AudioParams.Default.WithVolume(Math.Min(0, -10 * (1 - (ev.TotalIntensity - 200) / 800))));
        }
    }
}
