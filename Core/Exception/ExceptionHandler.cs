using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core
{
    public class ExceptionHandler
    {

        public static void Log(Exception ex)
        {
            if (Directory.Exists(@"C:\Temp"))
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"C:\Temp\MOLWsClient_errors_{0}.txt", DateTime.Now.ToString("ddMMyyyy")));
                using (StreamWriter sw = f.AppendText())
                {
                    sw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString("ddMMyyyy HH:mm:ss"), ex.Message));
                    sw.WriteLine(ex.StackTrace);

                    if (ex.InnerException != null)
                    {
                        sw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString("ddMMyyyy HH:mm:ss"), ex.Message));
                        sw.WriteLine(ex.StackTrace);
                    }
                }
            }
        }


        public static void Log(string message)
        {
            if (Directory.Exists(@"C:\Temp"))
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"C:\Temp\MOLWsClient_errors_{0}.txt", DateTime.Now.ToString("ddMMyyyy")));
                using (StreamWriter sw = f.AppendText())
                {
                    sw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString("ddMMyyyy HH:mm:ss"), message));
                }
            }
        }

    }
}
