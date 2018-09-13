using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POSM
{
    class CallbackThreads
    {


        public static void TimerCallbackThreading() { 
        
                 TimerCallback timerDelegate = new TimerCallback(Db_Backup);

                 System.Threading.Timer PumpTimer = new System.Threading.Timer(timerDelegate, null, 900000, 900000);
        
        }




        private static void Db_Backup(object state)
        {
            

            //Backup Script
        }

    }
}
