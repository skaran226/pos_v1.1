using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Collections;

namespace POSM
{
    public partial class Form1 : Form
    {
        public static Form1 fm;
        public Form1()
        {
            InitializeComponent();
            fm = this;
        }

        String str = "";
        // int j = 1;
        string trns_no = "";
        string reg_id = "";
        string time = "";
        string hot_key = "";

        ArrayList trim_arr = new ArrayList(); // collect all words without whitespace index value


        delegate void SetRichTextValue(object sender, SerialDataReceivedEventArgs e);

        SerialPort pos_port = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadPortProperties();

        }

        private void LoadPortProperties()
        {

            pos_port = new SerialPort();
            pos_port.PortName = "COM" + Setup.port_no;
            pos_port.BaudRate = Setup.baudrate;

            if (Setup.stopbits == "one") {

                pos_port.StopBits = StopBits.One;
            }


            if (Setup.parity == "none") {

                pos_port.Parity = Parity.None;
            }

            pos_port.DataBits = Setup.databits;

            if (Setup.handshake == "none") {

                pos_port.Handshake = Handshake.None;
            }

            pos_port.DataReceived += new SerialDataReceivedEventHandler(pos_received_data);

            try
            {

                pos_port.Open();
            }
            catch (Exception ex) {
                Console.WriteLine("COM"+Setup.port_no+" getting exception");
                Console.WriteLine(ex+"");
            }
        }

        private void pos_received_data(object sender, SerialDataReceivedEventArgs e)
        {
            //ArrayList arr = new ArrayList();
            int iBytesToRead = fm.pos_port.BytesToRead;
            byte[] comBuffer = new byte[iBytesToRead];
            int i;
            // byte[] buffer = new byte[readByte];

            try
            {
                fm.pos_port.Read(comBuffer, 0, iBytesToRead);
            }
            catch (Exception ex) {

                Console.WriteLine("does't read port data somthing error");
            }

            


            //Thread.Sleep(50);
            foreach (byte ch in comBuffer)
            {

                    // fm.richTextBox1.Text += (char)ch;
                    str += (char)ch;

            }





            if (str != "")
            {

                String[] sp_arr = str.Trim().Split();
                //string an_s = "";

                foreach (string s2 in sp_arr)
                {
                    if (s2 != " " && s2 != null && s2 != "")
                    {

                        trim_arr.Add(s2);

                    }
                }
                //string[] sp_arr2 = an_s.Split();
                for (i = 0; i < trim_arr.Count; i++)
                {
                    //richTextBox1.Text += "[" + i + "]:" + sp_arr[i] + "\n";
                    //Debug.Write("[" + i + "]:" + trim_arr[i].ToString() + "\n");
                    if (sp_arr[i].ToString() == "!")
                    {
                        time = trim_arr[i].ToString() + " " + trim_arr[i + 1].ToString();

                        //break;
                    }

                    if (trim_arr[i].ToString() == "TRAN#")
                    {
                        trns_no = trim_arr[i + 1].ToString();
                        break;

                    }



                }

                foreach (string s in Setup.hotList)
                {



                    if (str.ToLower().Contains(s))
                    {

                        hot_key = s;
                    }
                }

                if (trns_no != "")
                {
                    reg_id = trns_no.Substring(0, 3);
                }

                Console.Write(time);
                Console.Write(trns_no + "\n");
                Console.Write(hot_key + "\n");
                Console.Write(reg_id + "\n");

                if (fm.pos_data_box.InvokeRequired)
                {

                    SetRichTextValue st = new SetRichTextValue(pos_received_data);
                    fm.Invoke(st, new object[] { sender, e });

                }
                else
                {
                    fm.pos_data_box.Text += "Time:"+time + " Transaction:" + trns_no + " POSID:" + reg_id + " HotKey:" + hot_key + "\n";  
                }

                
                str = "";
                //Thread.Sleep(40);
                string query = "insert into posData values('" + trns_no + "','" + reg_id + "','" + hot_key + "','" + time + "');";
                DB.ExecuteNonQuery(query);

                trim_arr.Clear();
            }
        }
    }
}
