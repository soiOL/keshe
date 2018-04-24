using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using test1;
using test1.control;
using test1.data;

namespace test
{
    public class MotorLED
    {
        private List<byte> _motor = new List<byte>(_initMotor);
        private Post post;
        private GetData getData;

        private bool isMotorOn = false;

        private bool isLed1On = false;

        private bool isLed2On = false;

        private bool isLed3On = false;

        //InfoStruct infoStruct = new InfoStruct();
        readonly static byte[] _initMotor = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        //控制命令
        readonly byte[] _openMotor = {204, 238, 01, 09, 09, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _closeMotor = {204, 238, 01, 09, 11, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _openLed1 = {204, 238, 01, 09, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _closeLed1 = {204, 238, 01, 09, 02, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _openLed2 = {204, 238, 01, 09, 03, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _closeLed2 = {204, 238, 01, 09, 04, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _openLed3 = {204, 238, 01, 09, 05, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};
        readonly byte[] _closeLed3 = {204, 238, 01, 09, 06, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 255};


        public MotorLED(Post post, GetData getData)
        {
            this.getData = getData;
            this.post = post;
        }

        //控制传感器
        public void Write()
        {

            var motorLed = new MotorLED(post, getData);
            //BaseStruct temp = null;
            bool isWriting = false;
            BindStruct allowBind = new BindStruct();
            //socket收到数据时进行控制
            post.GetWebSocket().OnMessage += (sender, e) =>
            {
                if (!isWriting)
                {
                    isWriting = true;
                    getData.isReading = false; //停止发送数据
                    Console.WriteLine(e.Data);
                    JsonToObject jsonToObject = new JsonToObject(e.Data);

                    Console.WriteLine(jsonToObject._jsonType.ToString());

                    if (typeof(BindStruct) == (jsonToObject._jsonType))
                    {
                        BindStruct bindStruct = (BindStruct) jsonToObject.jsonObject;
                        string user = bindStruct.deviceId;
                        MessageBoxButtons messButton = MessageBoxButtons.OKCancel;

                        DialogResult dr = MessageBox.Show("是否允许" + user + "连接", "新用户连接", messButton);
                        if (dr == DialogResult.OK)
                        {

                            allowBind.action = "allowBind";
                            allowBind.deviceId = user;
                        }
                        else
                        {
                            allowBind.action = "forbid";
                            allowBind.deviceId = user;
                        }

                        post.PostData(allowBind);
                    }

                    if (typeof(BaseStruct) == (jsonToObject._jsonType))
                    {
                        //解析得到的控制命令
                        BaseStruct baseStruct = (BaseStruct) jsonToObject.jsonObject;
                        motorLed.ControlMotor(baseStruct, getData.serialPort);
                        motorLed.post.PostData(getData.lit);
                    }

                    getData.isReading = true;
                    isWriting = false;
                }

            };
        }

        public void ControlMotor(BaseStruct baseStruct, SerialPort serialPort)
        {

            if (baseStruct.Fan)
            {
                if (!isMotorOn)
                {
                    serialPort.Write(_openMotor, 0, 16);
                    //bool isGet = GetMotor(serialPort, 9);
                    //if(isGet)
                    //{
                    //    getData.lit.Fan = true;
                    //    isMotorOn = getData.lit.Fan;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Fan = true;
                    isMotorOn = getData.lit.Fan;
                }
            }
            else
            {
                if (isMotorOn)
                {
                    serialPort.Write(_closeMotor, 0, 16);
                    //bool isGet = GetMotor(serialPort, 11);
                    //if (isGet)
                    //{
                    //    getData.lit.Fan = false;
                    //    isMotorOn = getData.lit.Fan;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Fan = false;
                    isMotorOn = getData.lit.Fan;
                }



            }

            if (baseStruct.Led1)
            {
                if (!isLed1On)
                {
                    serialPort.Write(_openLed1, 0, 16);
                    //bool isGet = GetMotor(serialPort, 1);
                    //if (isGet)
                    //{
                    //    getData.lit.Led1 = true;
                    //    isLed1On = getData.lit.Led1;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Led1 = true;
                    isLed1On = getData.lit.Led1;
                }
            }
            else
            {
                if (isLed1On)
                {
                    serialPort.Write(_closeLed1, 0, 16);
                    //bool isGet = GetMotor(serialPort, 2);
                    //if (isGet)
                    //{
                    //    getData.lit.Led1 = false;
                    //    isLed1On = getData.lit.Led1;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Led1 = false;
                    isLed1On = getData.lit.Led1;
                }






            }

            if (baseStruct.Led2)
            {
                if (!isLed2On)
                {
                    serialPort.Write(_openLed2, 0, 16);
                    //bool isGet = GetMotor(serialPort, 3);
                    //if (isGet)
                    //{
                    //    getData.lit.Led2 = true;
                    //    isLed2On = getData.lit.Led2;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Led2 = true;
                    isLed2On = getData.lit.Led2;
                }






            }
            else
            {
                if (isLed2On)
                {
                    serialPort.Write(_closeLed2, 0, 16);
                    //bool isGet = GetMotor(serialPort, 4);
                    //if (isGet)
                    //{
                    //    getData.lit.Led2 = false;
                    //    isLed2On = getData.lit.Led2;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Led2 = false;
                    isLed2On = getData.lit.Led2;
                }
            }

            if (baseStruct.Led3)
            {
                if (!isLed3On)
                {
                    serialPort.Write(_openLed3, 0, 16);
                    //bool isGet = GetMotor(serialPort, 5);
                    //if (isGet)
                    //{
                    //    getData.lit.Led3 = true;
                    //    isLed3On = getData.lit.Led3;
                    //}
                    //else
                    //{
                    //    
                    //}
                    getData.lit.Led3 = true;
                    isLed3On = getData.lit.Led3;
                }
            }
            else
            {
                if (isLed3On)
                {
                    serialPort.Write(_closeLed3, 0, 16);
                    //bool isGet = GetMotor(serialPort , 6);

                    //    if (isGet)
                    //    {
                    //        getData.lit.Led3 = false;
                    //        isLed3On = getData.lit.Led3;
                    //    }
                    //    else
                    //    {
                    //        
                    //    }
                    //}
                    getData.lit.Led3 = false;
                    isLed3On = getData.lit.Led3;

                }
            }

            //private bool GetMotor(SerialPort serialPort, int flag)
            //{
            //    bool isGet = false;
            //    _motor.Clear();
            //    int i = 0;
            //    int j = 0;
            //    while (j < 3)
            //    {
            //        var temp = serialPort.ReadByte();
            //        _motor.Add((byte) temp);
            //        i++;
            //        if (i > 15)
            //        {
            //            j++;
            //            int flag2 = _motor[3];
            //            int flag3 = _motor[5];
            //            Console.WriteLine(_motor.ToString());
            //            i = 0;
            //            if (flag2 == 9 && flag3 == flag)
            //            {
            //                isGet = true;
            //                getData.lit.FanOk = true;
            //                getData.motorCount = 10;
            //                break;
            //            }
            //            else
            //            {
            //                _motor.Clear();
            //            }
            //        }

            //    }

            //    if (_motor.Count < 6)
            //        _motor = new List<byte>(_initMotor);
            //    return isGet;
            //}


        }
    }
}
