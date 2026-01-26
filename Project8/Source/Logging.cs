using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Project8.Source
{
    public class Logging
    {
        public enum Levels
        {
            INFO,
            WARN,
            ERROR
        }

        public static readonly string Filename = Path.Combine(Directory.GetCurrentDirectory(), "latest.logs");

        public static void Log(Levels level, string message)
        {
            if (File.Exists(Filename))
            {
                string lastLine = File.ReadLines(Filename).LastOrDefault();

                if (!string.IsNullOrWhiteSpace(lastLine))
                {
                    int start = lastLine.IndexOf('[') + 1;
                    int end = lastLine.IndexOf(']');
                    string dateStr = lastLine.Substring(start, end - start);

                    if (DateTime.TryParse(dateStr, out DateTime lastDate))
                    {
                        if (lastDate.Date < DateTime.UtcNow.Date)
                        {
                            File.WriteAllText(Filename, string.Empty);
                        }
                    }
                }
            }

            string msg = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level}] {message}\n";
            File.AppendAllText(Filename, msg);
        }
    }
}
