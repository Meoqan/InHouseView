using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kirche.inc.toolbox
{
    class logwriter
    {
        string message = string.Empty;
        int level = 0;
        DateTime date = DateTime.Now;

        static object threadlock = new object();
        static List<logwriter> loglist = new List<logwriter>();

        public static void wipe_log()
        {
            try
            {
                if (File.Exists("log.txt"))
                {
                    File.Delete("log.txt");
                }
            }
            catch
            {

            }
        }

        public static void add(string message, int level)
        {
            lock (threadlock)
            {
                logwriter lw = new logwriter();

                lw.message = message;
                lw.level = level;
                lw.date = DateTime.Now;

                loglist.Add(lw);
            }
        }

        public static void writelogtodisk()
        {
            List<logwriter> newloglist = new List<logwriter>();
            List<logwriter> oldloglist = null;
            lock (threadlock)
            {
                oldloglist = loglist;
                loglist = newloglist;
            }

            if (oldloglist.Count > 0)
            {
                try
                {
                    StreamWriter file = new StreamWriter("log.txt", true);

                    foreach (logwriter lw in oldloglist)
                    {
                        switch (lw.level)
                        {
                            case 1:
                                file.WriteLine(lw.date.ToString() + " > " + "ERROR  : " + lw.message);
                                break;
                            case 2:
                                file.WriteLine(lw.date.ToString() + " > " + "WARNING: " + lw.message);
                                break;
                            case 3:
                                file.WriteLine(lw.date.ToString() + " > " + "INFO   : " + lw.message);
                                break;
                            default:
                                file.WriteLine(lw.date.ToString() + " > " + "DEFAULT: " + lw.message);
                                break;
                        }
                    }

                    file.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: in logwriter.writelogtodisk() > " + e.ToString());
                }
            }
        }
    }
}
