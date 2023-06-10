namespace Content.Server.GameTicking.Events;

public sealed class PlayerBanedEvent : EntityEventArgs
{
    public string? AdminName { get; }
    public string Username { get; }
    public DateTimeOffset? Expires { get; }
    public string Reason { get; }
    public PlayerBanedEvent(string username, DateTimeOffset? expires, string reason, string? adminname = null)
    {
        AdminName = adminname;
        Username = username;
        Expires = expires;
        Reason = reason;
    }
}
