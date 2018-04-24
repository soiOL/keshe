using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using test;
using ZXing;

namespace test1
{
    public partial class CS : Form
    {
        public Post post;

        public CS()
        {
            post = new Post();
            InitializeComponent();
            
        }
        //void changelabel(Labelchange labelchange)
        //{
        //    label2.DataBindings.Add("Text", labelchange, "text");
        //    label2.DataBindings.Add("ForeColor", labelchange, "color");
        //}


        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            this.Visible = true;

            if (post.GetWebSocket().IsAlive)
            {
                post.GetWebSocket().Close();
            }
            //socket建立连接
            post.GetWebSocket().Connect();
            Console.WriteLine("socket connected");
            var getData = new GetData(post);

            //开始读取数据
            Thread th2 = new Thread(getData.GetDaTa);
            if (th2.IsAlive)
                th2.Abort();
            th2.Start();

            var write = new MotorLED(post, getData);

            //获取控制信号
            Thread th = new Thread(write.Write);
            if(th.IsAlive)
                th.Abort();
            th.Start();



        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //关闭socket并退出所有线程，关闭程序
            post.GetWebSocket().CloseAsync();
            Dispose();
            System.Environment.Exit(0);
        }

        private void 家居小助手_MouseClick(object sender, MouseEventArgs e)
        {
            //左键点击托盘图标显示主界面
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }          
        }

        private void 家居小工具_FormClosing(object sender, FormClosingEventArgs e)
        {
            //点击主界面的关闭按钮时取消关闭，并隐藏主界面，将程序挂在后台
            e.Cancel = true;
            this.Hide();
        }



        private void test_Shown(object sender, EventArgs e)
        {
            string t = BaseBoard.GetBaseBoardStrMd5();
            Console.WriteLine(t);
            Bitmap bitmap = GenByZXingNet(t);
            QR.Image = bitmap;

            


        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">二维码信息</param>
        /// <returns>图片</returns>
        Bitmap GenByZXingNet(string msg)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");//编码问题
            writer.Options.Hints.Add(
                EncodeHintType.ERROR_CORRECTION,
                ZXing.QrCode.Internal.ErrorCorrectionLevel.H

            );
            const int codeSizeInPixels = 200;   //设置图片长宽
            writer.Options.Height = writer.Options.Width = codeSizeInPixels;
            writer.Options.Margin = 0;//设置边框
            ZXing.Common.BitMatrix bm = writer.Encode(msg);
            Bitmap img = writer.Write(bm);
            return img;
        }

        private void CS_Load(object sender, EventArgs e)
        {

        }
    }

    public class Labelchange
    {
        public string text;
        //public Color color;
    }
}
