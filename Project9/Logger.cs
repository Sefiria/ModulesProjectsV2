using System;
using System.IO;

namespace Project9
{
    public static class Logger
    {
        private static readonly string LogPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pca_log.txt");
        public static void Init()
        {
            if (File.Exists(LogPath))
                File.Delete(LogPath);
        }
        public static void Write(string message)
        {
            return;// skip
            try
            {
                File.AppendAllText(LogPath, $"{DateTime.Now:HH:mm:ss} - {message}\n");
            }
            catch { /* ignorer les erreurs de log */ }
        }
    }
}
