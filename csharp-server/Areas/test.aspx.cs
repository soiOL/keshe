using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KS.Areas
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var strarr = SerialPort.GetPortNames();

            var serialPort = new SerialPort
            {
                PortName = strarr[0],
                BaudRate = 115200,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.Even
            };
            serialPort.Open();
            
            while (true)
            {
                Console.WriteLine(serialPort.ReadByte());
            }

        }



    }
}