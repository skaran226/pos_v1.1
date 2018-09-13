using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSM
{
    class Startup
    {
        public static string path = "C:/Users/Admin/Documents/";
        public static string org_pos_config = "pos.xml";
        public static string bak_pos_config = "pos_bak.xml";
        public static string org_hot = "hot.xml";
        public static string bak_hot = "hot_bak.xml";

        public static void SetUp() {   

            Backup.CheckFile(path + org_pos_config, path + bak_pos_config);
            Backup.CheckFile(path + org_hot, path + bak_hot);
            Setup.SetSerialProperties();
            Setup.LoadHotList();
          //  CallbackThreads.TimerCallbackThreading(); 
        }
    }
}
