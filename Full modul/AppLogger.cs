using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
using System.IO;

namespace Full_modul
{
    public static class AppLogger
    {
        private static readonly string _logDirectory;
        private static readonly string _dbLogPath;
        private static readonly string _appLogPath;
        private static readonly object _lock = new();

        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        private static LogLevel _minLogLevel = LogLevel.Info;

        static AppLogger()
        {
            string appDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "HRCalculator");

            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
                File.SetAttributes(appDataFolder, FileAttributes.Hidden);
            }

            _logDirectory = Path.Combine(appDataFolder, "Logs");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            _dbLogPath = Path.Combine(_logDirectory, $"db_log_{DateTime.Now:yyyyMMdd}.txt");
            _appLogPath = Path.Combine(_logDirectory, $"app_log_{DateTime.Now:yyyyMMdd}.txt");

            CleanOldLogs();
        }

        // Методы для установки уровня логирования
        public static void SetMinLogLevel(LogLevel level) => _minLogLevel = level;

        // Логирование подключения к БД (всегда записываются независимо от уровня)
        public static void LogDbInfo(string message) => Log("DB_INFO", message, _dbLogPath);
        public static void LogDbError(string message) => Log("DB_ERROR", message, _dbLogPath);
        public static void LogDbWarning(string message) => Log("DB_Warn", message, _dbLogPath);

        // Общее логирование приложения (с учетом уровня)
        public static void LogInfo(string message)
        {
            if (_minLogLevel <= LogLevel.Info)
                Log("INFO", message, _appLogPath);
        }

        public static void LogWarning(string message)
        {
            if (_minLogLevel <= LogLevel.Warning)
                Log("WARN", message, _appLogPath);
        }

        public static void LogError(string message) => Log("ERROR", message, _appLogPath);
        public static void LogDebug(string message)
        {
            if (_minLogLevel <= LogLevel.Debug)
                Log("DEBUG", message, _appLogPath);
        }

        public static void LogDbConnectionEvent(string message, ConnectionEventType eventType)
        {
            switch (eventType)
            {
                case ConnectionEventType.FirstConnect:
                    Log("DB_CONN", $"Первое подключение: {message}", _dbLogPath);
                    break;
                case ConnectionEventType.Reconnected:
                    Log("DB_CONN", $"Восстановлено: {message}", _dbLogPath);
                    break;
                case ConnectionEventType.Disconnected:
                    Log("DB_CONN", $"ОТКЛЮЧЕНИЕ: {message}", _dbLogPath);
                    break;
                case ConnectionEventType.StableStatus:
                    if (_minLogLevel <= LogLevel.Info)
                        Log("DB_STAT", $"Статус: {message}", _dbLogPath);
                    break;
            }
        }

        public enum ConnectionEventType
        {
            FirstConnect,
            Reconnected,
            Disconnected,
            StableStatus
        }

        private static void Log(string level, string message, string filePath)
        {
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(filePath,
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка записи в лог: {ex.Message}");
                }
            }
        }

        private static void CleanOldLogs()
        {
            try
            {
                foreach (var file in Directory.GetFiles(_logDirectory, "*.txt"))
                {
                    if (DateTime.Now - File.GetCreationTime(file) > TimeSpan.FromDays(7))
                    {
                        File.Delete(file);
                    }
                }
            }
            catch
            {

            }
        }
    }
}