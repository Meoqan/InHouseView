using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Kirche.inc.maintenance
{
    class control
    {
        static object threadlock = new object();
        static bool run = false;
        static Thread th = null;

        public static void threadstarter()
        {
            lock (threadlock)
            {
                run = true;

                th = new Thread(msec_1);
                th.Start();

                th = new Thread(msec_10);
                th.Start();

                th = new Thread(msec_100);
                th.Start();

                th = new Thread(msec_250);
                th.Start();

                th = new Thread(msec_1000);
                th.Start();

            }
        }


        public static void stop()
        {
            lock (threadlock)
            {
                run = false;
            }
        }

        static void msec_1()
        {
            bool running = true;

            while (running)
            {

                Thread.Sleep(1);

                // maintenance 
                

                lock (threadlock)
                {
                    running = run;
                }
            }
        }

        static void msec_10()
        {
            bool running = true;

            while (running)
            {

                Thread.Sleep(10);

                // maintenance 
                

                lock (threadlock)
                {
                    running = run;
                }
            }
        }

        static void msec_100()
        {
            bool running = true;

            while (running)
            {

                Thread.Sleep(100);

                // maintenance
                //player.Kplayer.update_network();

                lock (threadlock)
                {
                    running = run;
                }
            }
        }

        static void msec_250()
        {
            bool running = true;

            while (running)
            {

                Thread.Sleep(250);

                // maintenance 
                

                lock (threadlock)
                {
                    running = run;
                }
            }
        }

        static void msec_1000()
        {
            bool running = true;

            while (running)
            {

                Thread.Sleep(1000);

                // maintenance 
                toolbox.logwriter.writelogtodisk();

                lock (threadlock)
                {
                    running = run;
                }
            }
        }
    }
}
