using UnityEngine;

public static class Logger
{
    public enum LogType {Error, Warning, Info}

    private static void Log(LogType type, string message)
    {
        switch (type)
        {
            case LogType.Error: Debug.LogError(message); break;
            case LogType.Warning: Debug.LogWarning(message); break;
            case LogType.Info: Debug.Log(message); break;
        }
    }

    // ==== Обработчики ====
    
    public static void LogError(string message) => Log(LogType.Error, $"[ERROR]: {message}");
    public static void LogWarning(string message) => Log(LogType.Warning, $"[WARN]: {message}");
    public static void LogInfo(string message) => Log(LogType.Info, $"[INFO]: {message}");
}
