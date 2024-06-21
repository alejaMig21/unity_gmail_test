using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class EnvLoader
{
    private static readonly Dictionary<string, string> envVariables = new();

    static EnvLoader()
    {
        LoadEnvFile();
    }
    private static void LoadEnvFile()
    {
        string path = Path.Combine(Application.dataPath, ".env");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(".env file not found at " + path);
        }

        foreach (var line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var parts = line.Split(new[] { '=' }, 2);
            if (parts.Length == 2)
            {
                envVariables[parts[0].Trim()] = parts[1].Trim();
            }
        }
    }
    public static string GetEnvVariable(string key)
    {
        if (envVariables.TryGetValue(key, out string value))
        {
            return value;
        }
        throw new KeyNotFoundException($"Environment variable '{key}' not found");
    }
    public static void SetEnvVariable(string key, string value)
    {
        // if env var exists
        if (envVariables.ContainsKey(key))
        {
            envVariables[key] = value; // value updated
        }
        else
        {
            envVariables.Add(key, value); // var with value created
        }
        SaveEnvFile();
    }
    private static void SaveEnvFile()
    {
        string path = Path.Combine(Application.dataPath, ".env");
        using (StreamWriter writer = new(path))
        {
            foreach (var kvp in envVariables)
            {
                writer.WriteLine($"{kvp.Key}={kvp.Value}");
            }
        }
    }
}
