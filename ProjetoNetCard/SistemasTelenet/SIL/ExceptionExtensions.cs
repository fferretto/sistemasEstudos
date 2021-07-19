using System;
using System.IO;

namespace SIL
{
    public static class ExceptionExtensions
    {
        public static void SaveToFile(this Exception self, string filename)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GovernanceLog");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (string.IsNullOrEmpty(filename))
            {
                filename = string.Format("{0}.governance.log", DateTime.Now.Ticks);
                filename = Path.Combine(path, filename);
            }

            using (var writer = new StreamWriter(filename))
            {
                writer.WriteLine("Erro inexperado no servidor.");
                writer.WriteLine();
                writer.WriteLine("Exception type: " + self.GetType().FullName);
                writer.WriteLine("Exception message: " + self.Message);
                writer.WriteLine("Stack trace: " + self.StackTrace);
                writer.Flush();
                writer.Close();
            }
        }

        public static void SaveToFile(this Exception self)
        {
            self.SaveToFile(null);
        }
    }
}
