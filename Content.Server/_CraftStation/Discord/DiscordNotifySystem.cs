using Content.Server.GameTicking.Events;
using Content.Server.Maps;
using Content.Shared.Craft.CCVars;
using Robust.Shared.Configuration;
using System.Text.Json.Serialization;
using System.Net.Http;
using Content.Shared.GameTicking;
using Content.Shared.Administration;
using System.Text.Json;
using System.Text;
using Robust.Shared;

namespace Content.Server.Craft.Discord
{
    public sealed partial class DiscordNotifySystem : EntitySystem
    {
        [Dependency] private readonly IConfigurationManager _cfgMan = default!;
        [Dependency] private readonly IGameMapManager _mapMan = default!;

        private ISawmill _sawmill = default!;
        private readonly HttpClient _httpClient = new();

        private bool _enabled;
        private string _webhookRound = string.Empty;
        private string _webhookBan = string.Empty;

        public override void Initialize()
        {
            _cfgMan.OnValueChanged(CCVars.DiscordWebhooksEnabled, OnHookEnableChanged, true);
            _cfgMan.OnValueChanged(CCVars.DiscordRoundWebhook, OnRoundHookChanged, true);
            _cfgMan.OnValueChanged(CCVars.DiscordBanWebhook, OnBanHookChanged, true);

            SubscribeLocalEvent<RoundStartingEvent>(OnRoundStarting);
            SubscribeLocalEvent<RoundEndedEvent>(OnRoundEnding);
            SubscribeLocalEvent<PlayerBanedEvent>(OnPlayerBaned);

            _sawmill = IoCManager.Resolve<ILogManager>().GetSawmill("notifications");

            if (string.IsNullOrEmpty(_webhookRound)) return;
            SendPayload(_webhookRound, new WebhookPayload(Loc.GetString("discord-hook-server-start")));
        }

        #region CCVarEvents
        void OnHookEnableChanged(bool var)
        {
            _enabled = var;
        }
        void OnRoundHookChanged(string var)
        {
            _webhookRound = var;
        }
        void OnBanHookChanged(string var)
        {
            _webhookBan = var;
        }
        #endregion

        private async void SendPayload(string target,WebhookPayload payload)
        {
            var request = await _httpClient.PostAsync(target,
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            var content = await request.Content.ReadAsStringAsync();
            if (!request.IsSuccessStatusCode)
            {
                _sawmill.Log(LogLevel.Error, $"Discord returned bad status code when posting message: {request.StatusCode}\nResponse: {content}");
                return;
            }
        }

        private void OnRoundStarting(RoundStartingEvent ev)
        {
            if (string.IsNullOrEmpty(_webhookRound)) return;
            var map = _mapMan.GetSelectedMap();
            var payload = new WebhookPayload( Loc.GetString("discord-hook-round-start",
                ("roundid", ev.Id),
                ("mapname", map?.MapName ?? Loc.GetString("discord-hook-map-unknown")),
                ("timestamp", DateTimeOffset.Now.ToUnixTimeSeconds()))
            );
            SendPayload(_webhookRound, payload);
        }
        private void OnRoundEnding(RoundEndedEvent ev)
        {
            if (string.IsNullOrEmpty(_webhookRound)) return;
            var payload = new WebhookPayload(Loc.GetString("discord-hook-round-end",
                ("roundid", ev.RoundId),
                ("hours", ev.RoundDuration.Hours),
                ("minutes", ev.RoundDuration.Minutes)
            ));
            SendPayload(_webhookRound, payload);
        }
        private void OnPlayerBaned(PlayerBanedEvent ev)
        {
            if (string.IsNullOrEmpty(_webhookBan)) return;
            var payload = new WebhookPayload(ev.AdminName is not null ?
                Loc.GetString("discord-hook-player-ban-admin",
                ("player", ev.Username),
                ("reason", ev.Reason),
                ("timestamp", ev.Expires is null ? "never" : ev.Expires.Value.ToUnixTimeSeconds().ToString()),
                ("admin", ev.AdminName)) :
                Loc.GetString("discord-hook-player-ban-none",
                ("player", ev.Username),
                ("reason", ev.Reason),
                ("timestamp", ev.Expires is null ? "never" : ev.Expires.Value.ToUnixTimeSeconds().ToString()))
            );
            SendPayload(_webhookBan, payload);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            _cfgMan.UnsubValueChanged(CCVars.DiscordWebhooksEnabled, OnHookEnableChanged);
            _cfgMan.UnsubValueChanged(CCVars.DiscordRoundWebhook, OnRoundHookChanged);
            _cfgMan.UnsubValueChanged(CCVars.DiscordBanWebhook, OnBanHookChanged);
        }
    }

    public struct WebhookPayload
    {
        [JsonPropertyName("content")] public string Content { get; set; } = "";
        public WebhookPayload(string content)
        {
            Content = content;
        }
    }
}
