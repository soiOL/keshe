using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using test1;
using Label = System.Web.UI.WebControls.Label;

namespace test
{
    public class GetData
    {
        private bool isTahOn = false;  //判断温湿度是否有数据
        private int light; //光强
        private float hum; //湿度
        private float tem; //温度
        public bool isReading = true;
        public InfoStruct lit = new InfoStruct();
        public int motorCount = 0;
        public SerialPort serialPort;
        //private test1.test Test;
        private Post post;
        
        public GetData(Post post)
        {
            this.post = post;
        }
        public void GetDaTa()
        {
            try
            {
                //获取端口号
                var strArr = SerialPort.GetPortNames();

                foreach (var str in strArr)
                {
                    Console.WriteLine(str);
                }

                //string comName = Console.ReadLine();
                //选定端口并开启
                string comName = strArr[0];
                serialPort = new SerialPort(comName, 115200, Parity.None, 8, StopBits.One);
                serialPort.Open();
                List<byte> byteList = new List<byte>();     //用于存放读入的数据


                int tahCount = 0;
                int inCount = 0;
                int index = 0;

                while (true)
                {
                    if (isReading)
                    {
                        //从串口读数据
                        int temp = serialPort.ReadByte();
                        //Console.Write("{0} ", temp);
                        byteList.Add((byte)temp);
                        index++;
                        // 开头不是EE
                        if (byteList[0] != 238)
                        {
                            index = 0;
                            byteList.Clear();
                        }
                        //收到16个字节数据
                        if (index > 15)
                        {

                            //Console.WriteLine();
                            index = 0;

                            //判断是否为传感器的有效值
                            int isData = byteList[4];
                            if (isData == 1)
                            {
                                int flag = byteList[3];  //选定传感器
                                tahCount--;
                                inCount--;
                                motorCount--;

                                //数据处理
                                switch (flag)
                                {
                                    case 2:
                                        lit.IsOk = true;
                                        inCount = 10;
                                        int lh = byteList[5];
                                        int ll = byteList[6];
                                        //float light = (lh * 256.0f + ll);
                                        light = lh * 256 + ll;
                                        Console.WriteLine("光强: {0} lux", light);
                                        lit.Intensity = light;
                                        break;

                                    case 3:
                                        isTahOn = true;
                                        lit.TahOk = true;
                                        tahCount = 10;
                                        int th = byteList[5];
                                        int tl = byteList[6];
                                        int hh = byteList[7];
                                        int hl = byteList[8];
                                        tem = (th * 256 + tl) / 100.0f;
                                        hum = (hh * 256 + hl) / 100.0f;
                                        lit.Temperature = tem;
                                        lit.Humidity = hum;
                                        Console.WriteLine("温度: {0}℃\t湿度: {1}%", tem, hum);
                                        break;
                                    case 9:
                                        lit.FanOk = true;
                                        motorCount = 10;
                                        break;
                                }
                            }

                            //判断传感器是否在线
                            if (inCount < 0)
                                lit.IsOk = false;
                            if (tahCount < 0)
                                lit.TahOk = false;
                            if (motorCount < 0)
                                lit.FanOk = false;

                            //当温湿度有数据时将数据传输到服务器
                            if (isTahOn)
                            {
                                lit.BaseBoard = BaseBoard.GetBaseBoardStrMd5();
                                lit.Date = DateTime.Now.ToString();
                                post.PostData(lit);
                            }


                            isTahOn = false;


                            byteList.Clear();

                        }
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                MessageBoxButtons messageBox = MessageBoxButtons.RetryCancel;
                DialogResult dr = MessageBox.Show("无法找到端口", "错误", messageBox);
                if (dr == DialogResult.Retry)
                {
                    GetDaTa();
                }
                else
                {
                    //关闭socket并退出所有线程，关闭程序
                    post.GetWebSocket().CloseAsync();
                    System.Environment.Exit(0);
                }

                //throw;
            }
            
        }

    }

}

