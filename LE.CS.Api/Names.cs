namespace LE.CS.Api;

public static class Names
{
    public static bool Valid(string name) => name.Length is >= 1 and <= 100 && name.All(c => char.IsAsciiLetterOrDigit(c) || c is '_' or '-');
}
