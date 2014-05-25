using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kirche.inc
{
    class main
    {
        public static StartupForm loading = new StartupForm();
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif

        
        static void Main()
        {
            
            
            toolbox.logwriter.wipe_log();

            toolbox.logwriter.add("WELCOME TO Kirche v0.0", 3);
            inc.maintenance.control.threadstarter();

            using (var program = new inc.engine.Window())
                program.Run();
        }
    }
}
