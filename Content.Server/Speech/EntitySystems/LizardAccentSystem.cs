using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class LizardAccentSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LizardAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, LizardAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // hissss
        message = Regex.Replace(message, "s+", "sss");
        // hiSSS
        message = Regex.Replace(message, "S+", "SSS");
        // ekssit
        message = Regex.Replace(message, @"(\w)x", "$1kss");
        // ecks
        message = Regex.Replace(message, @"\bx([\-|r|R]|\b)", "ecks$1");
        // eckS
        message = Regex.Replace(message, @"\bX([\-|r|R]|\b)", "ECKS$1");

        // c => ссс
        message = Regex.Replace(message, "с+", "сс");
        // С => CCC
        message = Regex.Replace(message, "С+", "Ссс");
        // з => ссс
        message = Regex.Replace(message, "з+", "сс");
        // З => CCC
        message = Regex.Replace(message, "З+", "Ссс");
        // ш => шшш
        message = Regex.Replace(message, "ш+", "шш");
        // Ш => ШШШ
        message = Regex.Replace(message, "Ш+", "Шшш");
        // ч => щщщ
        message = Regex.Replace(message, "ч+", "щщ");
        // Ч => ЩЩЩ
        message = Regex.Replace(message, "Ч+", "Щщщ");

        args.Message = message;
    }
}
