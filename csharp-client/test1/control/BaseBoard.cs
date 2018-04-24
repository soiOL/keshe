using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class BaseBoard
    {
        //获取主板号的MD5编码
        public static string GetBaseBoardStrMd5()
        {
            ManagementClass mc = new ManagementClass("Win32_BaseBoard");
            ManagementObjectCollection moc = mc.GetInstances();
            string strID = null;
            foreach (ManagementObject mo in moc)
            {
                strID = mo.Properties["SerialNumber"].Value.ToString();
                break;
            }
            return GetMD5String(strID);

        }
        private static string GetbyteToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static string GetMD5String(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] data2 = md5.ComputeHash(data);

            return GetbyteToString(data2);
        }

    }
}
