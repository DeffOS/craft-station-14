using Robust.Shared;
using Robust.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.Craft.CCVars
{
    [CVarDefs]
    public sealed class CCVars : CVars
    {
        public static readonly CVarDef<bool> DiscordWebhooksEnabled =
            CVarDef.Create("discord.webhooks_enabled", false, CVar.SERVERONLY);

        public static readonly CVarDef<string> DiscordRoundWebhook =
            CVarDef.Create("discord.webhook_round", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);
        public static readonly CVarDef<string> DiscordBanWebhook =
            CVarDef.Create("discord.webhook_ban", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);
    }
}
