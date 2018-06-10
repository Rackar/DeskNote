using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DeskNote
{
    public partial class DesktopNote : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        DesktopNoteSetting dns = new DesktopNoteSetting();
        //System.Timers.Timer timer = new System.Timers.Timer();
        System.Threading.Timer threadTimer;
        int currentCount = 0;
        //定义委托
          public delegate void SetControlValue(object value);

        public DesktopNote()
        {
            InitializeComponent();
            //恢复位置
            readSet();
        }
        void readSet()
        {
            dns.readConfig();
            this.Opacity = dns.op/100;
            //设置窗体位置和大小
            this.StartPosition = FormStartPosition.Manual;
            this.Top = dns.wintop;
            this.Left = dns.winleft;
            this.Size = new Size(dns.winwidth, dns.winheight);
            richTextBox1.Font = new Font(dns.fontname, (float)dns.fSize, (FontStyle)dns.fStyle);
            richTextBox1.ForeColor = ColorTranslator.FromHtml(dns.fColor);
            this.BackColor = ColorTranslator.FromHtml(dns.bgColor);
            richTextBox1.BackColor = this.BackColor;
            panel_right.Width = 2;
            panel_right.Height = this.Size.Height - 25;
            panel_right.Left = this.Width - 6;
            panel_down.Height = 2;
            panel_down.Width = this.Size.Width - 2;
            panel_down.Top = this.Height - 22;
            if(dns.autosave=="t")
                autoRunToolStripMenuItem.Checked = true;
            else if (dns.autosave =="f")
            autoRunToolStripMenuItem.Checked = false;
            //       panel_rightdown.Size = new Size(1, 1);

            richTextBox1.Text = dns.sb.ToString();

           
            
        }
        private void TimerUp(object value)
         {
            currentCount += 1;
            this.Invoke(new SetControlValue(writeSetTimer), currentCount);
         }

    void writeSetTimer(object o )
        {
            writeSet();
        }

        void writeSet()
        {
            dns.winleft = this.Left;
            dns.wintop= this.Top;
            dns.winwidth = this.Size.Width;
            dns.winheight = this.Size.Height;
            dns.fSize = (int)this.richTextBox1.Font.Size;
            dns.fontname = richTextBox1.Font.FontFamily.Name.ToString();
            dns.fStyle = (int)(richTextBox1.Font.Style);
            dns.fColor = ColorTranslator.ToHtml(richTextBox1.ForeColor);
            dns.bgColor = ColorTranslator.ToHtml(this.BackColor);
            dns.op = this.Opacity*100;
             //  string s = richTextBox1.Text.Replace("\n", "\r\n");
            string s = richTextBox1.Text;
            dns.sb.Remove(0, dns.sb.Length);
            dns.sb.Append(s);



            dns.writeConfig();

        }


        private void DesktopNote_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            
        }

       

        private void btn_mini_Click(object sender, EventArgs e)
        {
            
            this.WindowState = FormWindowState.Minimized;
            //隐藏任务栏区图标
            this.ShowInTaskbar = false;
            //图标显示在托盘区
            notifyIcon1.Visible = true;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DesktopNote_Load(object sender, EventArgs e)
        {

            threadTimer = new System.Threading.Timer(new TimerCallback(TimerUp), null, Timeout.Infinite, 1000);
            threadTimer.Change(0, 30000);


            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = true;
            }
        }

        private void DesktopNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer.Stop();


            if (MessageBox.Show("Do you want to exit？", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                writeSet();
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dns.writeConfig();
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesktopNoteOption dno = new DesktopNoteOption();
            dno.dns = this.dns;


            if(dno.ShowDialog()==DialogResult.Cancel)
            {
                richTextBox1.Font =dno. myfont;
                richTextBox1.ForeColor = dno.fColor;
                this.BackColor = dno.bgColor;
                richTextBox1.BackColor = this.BackColor;
                this.Opacity = dno.op/100;
                dns.pathtwo = dno.path;
                //MessageBox.Show(dno.s);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void panel_right_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
               // this.Width = this.PointToClient(MousePosition).X;
                this.Size = new Size(this.PointToClient(MousePosition).X, this.Size.Height);
                panel_down.Width = this.Size.Width-2;
            }
        }

        private void panel_down_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Size = new Size(this.Size.Width, this.PointToClient(MousePosition).Y);
                //this.Height = this.PointToClient(MousePosition).Y;
                panel_right.Height = this.Size.Height-20;
            }
        }

        private void autoRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dns.autosave=="f")
            {
                dns.autosave = "t";
                autoRunToolStripMenuItem.Checked = true;
                SetStartup();
            }
            else if (dns.autosave=="t")
            {
                dns.autosave = "f";
                autoRunToolStripMenuItem.Checked = false;
                SetStartup();
            }
            
        }
        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (dns.autosave == "t")
                rk.SetValue("DeskNote", Application.ExecutablePath.ToString());
            else if (dns.autosave == "f")
                rk.DeleteValue("DeskNote", false);
            rk.Close();
        }

        private void panel_right_MouseHover(object sender, EventArgs e)
        {
            panel_right.BackColor = Color.Black;
        }

        private void panel_down_MouseHover(object sender, EventArgs e)
        {
            panel_down.BackColor = Color.Black;
        }

        private void panel_down_MouseLeave(object sender, EventArgs e)
        {
            panel_down.BackColor = Color.Transparent;
        }

        private void panel_right_MouseLeave(object sender, EventArgs e)
        {
            panel_right.BackColor = Color.Transparent;
        }
    }
   
}
