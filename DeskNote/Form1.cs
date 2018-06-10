using DeskNote.Properties;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DeskNote
{
    public partial class Form1 : Form
    {
       public DesktopNoteSetting dns = new DesktopNoteSetting();
        //System.Timers.Timer timer = new System.Timers.Timer();
        System.Threading.Timer threadTimer;
        int currentCount = 0;
       public bool isAnyChange = false;
        bool isInited = false;
        //定义委托
        public delegate void SetControlValue(object value);
        public drawForm childForm;//此为副窗体
        public Form1()
        {
            InitializeComponent();
          
            this.Opacity = 0.3; // 窗体透明度                
            this.childForm = new drawForm(this);

            this.childForm.BringToFront();
            this.childForm.Location = new Point(this.Location.X + 8, this.Location.Y + 29);
            this.childForm.Size = new Size(this.Size.Width - 17, this.Height - 39);
            this.childForm.ShowInTaskbar = false;
            this.childForm.Show();
            this.childForm.Owner = this;    // 这支所属窗体                
            this.childForm.Dock = DockStyle.Fill;

            readSet();

        }
        void readSet()
        {
            dns.readConfig();
            this.Opacity = dns.op / 100;
            //设置窗体位置和大小
            this.StartPosition = FormStartPosition.Manual;
            this.Top = dns.wintop;
            this.Left = dns.winleft;
            this.Size = new Size(dns.winwidth, dns.winheight);
            childForm.richTextBox1.Font = new Font(dns.fontname, (float)dns.fSize, (FontStyle)dns.fStyle);
            childForm.richTextBox1.ForeColor = ColorTranslator.FromHtml(dns.fColor);
            childForm.label1.Font= new Font(dns.fontname, (float)dns.fSize, (FontStyle)dns.fStyle);
            childForm.label1.ForeColor = ColorTranslator.FromHtml(dns.fColor);

            this.BackColor = ColorTranslator.FromHtml(dns.bgColor);
            if (dns.autosave == "t")
                autoRunToolStripMenuItem.Checked = true;
            else if (dns.autosave == "f")
                autoRunToolStripMenuItem.Checked = false;
            //       panel_rightdown.Size = new Size(1, 1);

            if (dns.staytop == "t")
            {
                stayTopToolStripMenuItem.Checked = true;
                this.TopMost = true;
            }
            else if (dns.staytop == "f")
            {
                this.TopMost = false;
                stayTopToolStripMenuItem.Checked = false;
            }

            if (dns.autoremote == "t")
                autoSYNCToolStripMenuItem.Checked = true;
            else if (dns.autoremote == "f")
                autoSYNCToolStripMenuItem.Checked = false;


            pathToolStripMenuItem.Text = dns.remotepath;




            childForm.richTextBox1.Text = dns.sb.ToString();
            childForm.label1.Text = childForm.richTextBox1.Text;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            threadTimer = new System.Threading.Timer(new TimerCallback(TimerUp), null, Timeout.Infinite, 1000);
            //1秒调用一次
            threadTimer.Change(0, 1000);
            isInited = true;
        }
        private void TimerUp(object value)
        {
            currentCount += 1;
            this.Invoke(new SetControlValue(writeSetTimer), currentCount);
        }

        void writeSetTimer(object o)
        {
            if(isAnyChange)
            writeSet();
        }

        void writeSet()
        {
            dns.winleft = this.Left;
            dns.wintop = this.Top;
            dns.winwidth = this.Size.Width;
            dns.winheight = this.Size.Height;
            dns.fSize = (int)childForm.richTextBox1.Font.Size;
            dns.fontname = childForm.richTextBox1.Font.FontFamily.Name.ToString();
            dns.fStyle = (int)(childForm.richTextBox1.Font.Style);
            dns.fColor = ColorTranslator.ToHtml(childForm.richTextBox1.ForeColor);
            dns.bgColor = ColorTranslator.ToHtml(this.BackColor);
            dns.op = this.Opacity * 100;
            string s = childForm.richTextBox1.Text.Replace("\n", "\r\n");
            //string s = richTextBox1.Text;
            dns.sb.Remove(0, dns.sb.Length);
            dns.sb.Append(s);



            dns.writeConfig();
            isAnyChange = false;

        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.childForm != null)
                this.childForm.Size = new Size(this.Size.Width - 17, this.Height - 39);
            if(isInited)
            writeSet();
        }
        //副窗体随主窗体位置移动  
        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            if (this.childForm != null)
                childForm.Location = new Point(this.Location.X + 8, this.Location.Y + 29);
            if (isInited)
                writeSet();
        }



       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            childForm.richTextBox1.Visible = true;
            childForm.richTextBox1.Focus();
            childForm.richTextBox1.Select();

        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Point formPoint = this.PointToClient(Control.MousePosition);//鼠标相对于窗体左上角的坐标
            if(formPoint.X>=0&& formPoint.X<=this.Size.Width-5&& formPoint.Y >= 0 && formPoint.Y <= this.Size.Height-30)
            {
               // MessageBox.Show(String.Format(" {0} {1} ",formPoint.X,formPoint.Y));
            }
            else
            {
                childForm.richTextBox1.Visible = false;
                childForm.label1.Text = childForm.richTextBox1.Text;
            }
            
        }

        private void Form1_MinimumSizeChanged(object sender, EventArgs e)
        {
           if( this.WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
            
            
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.TopMost = false;
            
                writeSet();
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            
            
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesktopNoteOption dno = new DesktopNoteOption(this);
            dno.dns = this.dns;
            this.TopMost = false;

            if (dno.ShowDialog() == DialogResult.Cancel)
            {
               childForm. richTextBox1.Font = dno.myfont;
                childForm.richTextBox1.ForeColor = dno.fColor;
                childForm.label1.Font = dno.myfont;
                childForm.label1.ForeColor = dno.fColor;

                this.BackColor = dno.bgColor;
              //  childForm.richTextBox1.BackColor = this.BackColor;
                this.Opacity = dno.op==0? 0.01 : dno.op / 100;
                dns.pathtwo = dno.path;
                //MessageBox.Show(dno.s);
                writeSet();
                this.TopMost = stayTopToolStripMenuItem.Checked;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void autoRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dns.autosave == "f")
            {
                dns.autosave = "t";
                autoRunToolStripMenuItem.Checked = true;
                SetStartup();
            }
            else if (dns.autosave == "t")
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            MessageBox.Show(" Bugs contact to Yangxu8410@gmail.com","DeskNote v1.2",MessageBoxButtons.OK, MessageBoxIcon.Information);
             this.TopMost = stayTopToolStripMenuItem.Checked;
        }

        private void stayTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dns.staytop == "f")
            {
                dns.staytop = "t";
                stayTopToolStripMenuItem.Checked = true;
                this.TopMost = true;
            }
            else if (dns.staytop == "t")
            {
                dns.staytop = "f";
                stayTopToolStripMenuItem.Checked = false;
                this.TopMost = false;
            }
        }

        private void autoSYNCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ;
            if (dns.autoremote == "f")
            {
                dns.autoremote = "t";
                autoSYNCToolStripMenuItem.Checked = true;
                
            }
            else if (dns.autoremote == "t")
            {
                dns.autoremote = "f";
                autoSYNCToolStripMenuItem.Checked = false;
                
            }
        }

        private void setRemotePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = dns.remotepath;
            sfd.Filter = @"text|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                
            dns.remotepath = sfd.FileName;
            pathToolStripMenuItem.Text = dns.remotepath;
            writeSet();
            }
        }
    }

}
