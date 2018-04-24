using System.Reflection.Emit;
using System.Windows.Forms;

namespace test1
{
    partial class CS
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CS));
            this.LOT = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.QR = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QR)).BeginInit();
            this.SuspendLayout();
            // 
            // LOT
            // 
            resources.ApplyResources(this.LOT, "LOT");
            this.LOT.ContextMenuStrip = this.contextMenuStrip1;
            this.LOT.MouseClick += new System.Windows.Forms.MouseEventHandler(this.家居小助手_MouseClick);
            // 
            // contextMenuStrip1
            // 
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem,
            this.退出ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // 退出ToolStripMenuItem
            // 
            resources.ApplyResources(this.退出ToolStripMenuItem, "退出ToolStripMenuItem");
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.toolStripTextBox1_Click);
            // 
            // 退出ToolStripMenuItem1
            // 
            resources.ApplyResources(this.退出ToolStripMenuItem1, "退出ToolStripMenuItem1");
            this.退出ToolStripMenuItem1.Name = "退出ToolStripMenuItem1";
            this.退出ToolStripMenuItem1.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // QR
            // 
            resources.ApplyResources(this.QR, "QR");
            this.QR.Name = "QR";
            this.QR.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CS
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QR);
            this.ForeColor = System.Drawing.SystemColors.Desktop;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.家居小工具_FormClosing);
            this.Load += new System.EventHandler(this.CS_Load);
            this.Shown += new System.EventHandler(this.test_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.QR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon LOT;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem1;
        private System.Windows.Forms.PictureBox QR;
        private System.Windows.Forms.Label label1;

        //public System.Windows.Forms.Label GetLabel()
        //{
        //    return label2;
        //}

        //public void SetLabel(System.Windows.Forms.Label label)
        //{
        //    this.label2 = label;
        //}

        
    }
}

