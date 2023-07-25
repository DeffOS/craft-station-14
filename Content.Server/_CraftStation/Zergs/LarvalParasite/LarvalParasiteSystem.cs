using Content.Server.Popups;
using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.CraftStation.Zergs.LarvalParasite
{
    public sealed class LarvalParasiteSystem : EntitySystem
    {

        [Dependency] private readonly IGameTiming _gTime = default!;
        [Dependency] private readonly IPrototypeManager _protoMan = default!;
        [Dependency] private readonly SharedActionsSystem _actSys = default!;
        [Dependency] private readonly MobStateSystem _stateSys = default!;
        [Dependency] private readonly PopupSystem _popSys = default!;
        [Dependency] private readonly DamageableSystem _dmgSys = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<LarvalInfestationComponent, ComponentInit>(OnHostInit);

            SubscribeLocalEvent<LarvalParasiteComponent, ComponentInit>(OnComponentInit);
            SubscribeLocalEvent<LarvalParasiteComponent, LarvalParasiteConsumeEvent>(OnConsume);
            SubscribeLocalEvent<LarvalParasiteComponent, LarvalParasiteKillHostEvent>(OnKillHost);
            SubscribeLocalEvent<LarvalParasiteComponent, LarvalParasiteInfestHostEvent>(OnInfestHost);
            SubscribeLocalEvent<LarvalParasiteComponent, LarvalParasiteEvolveEvent>(OnEvolve);
        }

        #region Host
        private void OnHostInit(EntityUid uid, LarvalInfestationComponent component, ComponentInit args)
        {

        }

        #endregion

        #region Parasite
        private void OnComponentInit(EntityUid uid, LarvalParasiteComponent component, ComponentInit args)
        {
            ToggleHostRelatedActions(uid, component);
            component.KillHostAction.Cooldown = (_gTime.CurTime, _gTime.CurTime +
                component.KillHostAction.UseDelay ?? TimeSpan.FromSeconds(600));
            component.InfestHostAction.Cooldown = (_gTime.CurTime, _gTime.CurTime +
                component.InfestHostAction.UseDelay ?? TimeSpan.FromSeconds(600));
        }

        private void OnConsume(EntityUid uid, LarvalParasiteComponent component, LarvalParasiteConsumeEvent args)
        {
            var host = component.HostEntity;
            if (host is null) return;
            if (_stateSys.IsDead(host.Value))
            {
                _popSys.PopupEntity("zerg-parasite-consume-hostdead", uid, uid, Shared.Popups.PopupType.SmallCaution);
                return;
            }
            if (_dmgSys.TryChangeDamage(host, component.ConsumeDamage, true) is not null)
            {
                // Actually reward player with gene points
                // Also create sound for parasite only to indicate succsess
            }
            else
            {
                //Create sound for parasite only to indicate falure to dispence damage
            }
        }
        private void OnKillHost(EntityUid uid, LarvalParasiteComponent component, LarvalParasiteKillHostEvent args)
        {
            var host = component.HostEntity;
            if (host is null) return;
            _protoMan.TryIndex<DamageTypePrototype>("Slash", out var dmgType);
            if (dmgType is null) return;
            var dmg = new DamageSpecifier(dmgType, 75);
            _dmgSys.TryChangeDamage(host, dmg, true);
            DeattachFromHost(uid, component);
        }
        private void OnInfestHost(EntityUid uid, LarvalParasiteComponent component, LarvalParasiteInfestHostEvent args)
        {
            if (component.HostEntity is null) return;
        }
        private void OnEvolve(EntityUid uid, LarvalParasiteComponent component, LarvalParasiteEvolveEvent args)
        {

        }

        private void ToggleHostRelatedActions(EntityUid uid, LarvalParasiteComponent component, bool? desired = null)
        {
            if (desired is not null ? desired.Value : component.HostEntity.HasValue)
            {
                _actSys.SetEnabled(component.ConsumeAction, true);
                _actSys.AddAction(uid, component.ConsumeAction, uid);
                _actSys.SetEnabled(component.KillHostAction, true);
                _actSys.AddAction(uid, component.KillHostAction, uid);
                _actSys.SetEnabled(component.InfestHostAction, true);
                _actSys.AddAction(uid, component.InfestHostAction, uid);
            }
            else
            {
                _actSys.SetEnabled(component.ConsumeAction, false);
                _actSys.RemoveAction(uid, component.ConsumeAction);
                _actSys.SetEnabled(component.KillHostAction, false);
                _actSys.RemoveAction(uid, component.KillHostAction);
                _actSys.SetEnabled(component.InfestHostAction, false);
                _actSys.RemoveAction(uid, component.InfestHostAction);
            }
        }

        private void DeattachFromHost(EntityUid uid, LarvalParasiteComponent component)
        {

        }
        #endregion
    }
    public sealed class LarvalParasiteConsumeEvent : InstantActionEvent { }
    public sealed class LarvalParasiteKillHostEvent : InstantActionEvent { }
    public sealed class LarvalParasiteInfestHostEvent : InstantActionEvent { }
    public sealed class LarvalParasiteEvolveEvent : InstantActionEvent { }
}
