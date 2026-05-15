namespace JobApplicationTracker.Wpf.Services;

public static class SessionStore
{
    public static string? Token { get; private set; }

    public static void SetToken(string token) => Token = token;

    public static void Clear() => Token = null;
}