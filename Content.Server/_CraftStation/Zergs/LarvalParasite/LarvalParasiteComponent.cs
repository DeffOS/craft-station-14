// using Content.Shared.Actions;
// using Content.Shared.Actions.ActionTypes;
// using Content.Shared.Damage;
// using Robust.Shared.Utility;

// namespace Content.Server.CraftStation.Zergs.LarvalParasite
// {
//     [RegisterComponent]
//     public sealed class LarvalParasiteComponent : Component
//     {
//         [ViewVariables]
//         public EntityUid? HostEntity;

//         [DataField("consumeDamage", required: true)]
//         [ViewVariables(VVAccess.ReadWrite)]
//         public DamageSpecifier ConsumeDamage = new();

//         [DataField("consumeReward", required: true)]
//         [ViewVariables(VVAccess.ReadWrite)]
//         public int ConsumeReward = 2;

//         [DataField("consumeAction")]
//         public InstantAction ConsumeAction = new()
//         {
//             UseDelay = TimeSpan.FromSeconds(15),
//             Icon = new SpriteSpecifier.Texture(new("Structures/Walls/solid.rsi/full.png")),
//             ItemIconStyle = ItemActionIconStyle.NoItem,
//             DisplayName = "zerg-parasite-consume",
//             Description = "zerg-parasite-consume-desc",
//             Priority = -1,
//             Event = new LarvalParasiteConsumeEvent(),
//         };

//         [DataField("killHostReward", required: true)]
//         [ViewVariables(VVAccess.ReadWrite)]
//         public int KillHostReward = 20;

//         [DataField("killHostAction")]
//         public InstantAction KillHostAction = new()
//         {
//             UseDelay = TimeSpan.FromSeconds(600),
//             Icon = new SpriteSpecifier.Texture(new("Structures/Walls/solid.rsi/full.png")),
//             ItemIconStyle = ItemActionIconStyle.NoItem,
//             DisplayName = "zerg-parasite-killhost",
//             Description = "zerg-parasite-killhost-desc",
//             Priority = -1,
//             Event = new LarvalParasiteKillHostEvent(),
//         };

//         [DataField("infestHostAction")]
//         public InstantAction InfestHostAction = new()
//         {
//             Enabled = false,
//             UseDelay = TimeSpan.FromSeconds(600),
//             Icon = new SpriteSpecifier.Texture(new("Structures/Walls/solid.rsi/full.png")),
//             ItemIconStyle = ItemActionIconStyle.NoItem,
//             DisplayName = "zerg-parasite-infesthost",
//             Description = "zerg-parasite-infesthost-desc",
//             Priority = -1,
//             Event = new LarvalParasiteInfestHostEvent(),
//         };

//         [DataField("evolveAction")]
//         public InstantAction EvolveAction = new()
//         {
//             Enabled = false,
//             UseDelay = TimeSpan.FromSeconds(60),
//             Icon = new SpriteSpecifier.Texture(new("Structures/Walls/solid.rsi/full.png")),
//             ItemIconStyle = ItemActionIconStyle.NoItem,
//             DisplayName = "zerg-parasite-evolve",
//             Description = "zerg-parasite-evolve-desc",
//             Priority = -1,
//             Event = new LarvalParasiteEvolveEvent(),
//         };
//     }
// }
