// NetworkConnectionData.cs
public static class NetworkConnectionData
{
    // Renamed the enum to avoid ambiguity
    public enum NetworkConnectionType { None, Host, Client }

    // Property now uses the renamed enum
    public static NetworkConnectionType ConnectionType { get; set; } = NetworkConnectionType.None;

    // Optional additional data
    public static string PlayerName { get; set; }
    public static string LobbyCode { get; set; }
}