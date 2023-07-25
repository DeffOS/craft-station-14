using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.CraftStation.Zergs.LarvalParasite
{
    [AdminCommand(AdminFlags.Spawn | AdminFlags.Debug)]
    public sealed class LarvalParasiteCommand : IConsoleCommand
    {
        [Dependency] private readonly IPlayerManager _players = default!;

        public string Command => "larvalinfection";
        public string Description => "zerg-command-infect-desc";
        public string Help => "zerg-command-infect-help";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 2)
            {
                shell.WriteError(Loc.GetString("zerg-command-infect-err-noargs"));
                return;
            }

            if (!_players.TryGetSessionByUsername(args[0], out var parasite))
            {
                shell.WriteError(Loc.GetString("zerg-command-infect-err-noparas"));
                return;
            }

            if (!_players.TryGetSessionByUsername(args[1], out var host))
            {
                shell.WriteError(Loc.GetString("zerg-command-infect-err-nohost"));
                return;
            }


        }

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1 || args.Length == 2)
            {
                return CompletionResult.FromHintOptions(
                    CompletionHelper.SessionNames(players: _players),
                    "<playerName>");
            }

            return CompletionResult.Empty;
        }
    }
}
