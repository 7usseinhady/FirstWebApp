
namespace WebApp.SharedKernel.Helpers
{
    public class FileExLogger
    {
        public static bool Log(string message)
        {
            try
            {
                List<bool> lIndicator = new List<bool>();
                var fileName = $"{DateTime.UtcNow.ToString("ddMMyyyy")}.txt";

                //string dir = Path.GetTempPath();
                //lIndicator.Add(Write(dir, fileName, message));

                string dir = @"wwwroot\Logs\";
                lIndicator.Add(Write(dir, fileName, message));

                return lIndicator.All(x => x);
            }
            catch
            {
                return false;
            }
        }

        public static bool Write(string dir, string fileName, string message)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                FileStream filestream = new FileStream(string.Format("{0}\\{1}", dir, fileName), FileMode.Append, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter((Stream)filestream);
                streamWriter.WriteLine(message);
                streamWriter.Close();
                filestream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
