using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DeskNote
{
    public partial class DesktopNoteOption : Form
    {
        public  Font myfont = new Font(new FontFamily("Arial"), (float)14f, FontStyle.Regular);
        public Color fColor = Color.Black;
        public Color bgColor = Color.BlueViolet;
        public double op = 100;
        public string path = @"D:\desknote.txt";
        public string s = "";
        public DesktopNoteSetting dns = new DesktopNoteSetting();
        public Form1 f;

        public DesktopNoteOption(Form1 fin)
        {
            InitializeComponent();
            f = fin;
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            //fd.Font = fontConfigLabel.Font;
            fd.Font = myfont;

            var result = fd.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                myfont = fd.Font;
                
                btn_font.Text = myfont.Name;

                refreshSetting();

            }
        }

        private void num_FontSize_ValueChanged(object sender, EventArgs e)
        {
           // myfont.Size = (float)num_FontSize.Value;
          //  richTextBox1.Font.Size = num_FontSize.Value;
            myfont= new Font(myfont.FontFamily, (float)num_FontSize.Value, myfont.Style);
            refreshSetting();
        }

        private void DesktopNoteOption_Load(object sender, EventArgs e)
        {
            myfont = new Font(dns.fontname, (float)dns.fSize, (FontStyle)dns.fStyle);
            richTextBox1.Font = new Font(dns.fontname, (float)dns.fSize, (FontStyle)dns.fStyle);
            richTextBox1.ForeColor = ColorTranslator.FromHtml(dns.fColor);
            richTextBox1.BackColor = ColorTranslator.FromHtml(dns.bgColor);
            fColor = ColorTranslator.FromHtml(dns.fColor);
            bgColor = ColorTranslator.FromHtml(dns.bgColor);
            op = dns.op;





            numericUpDown1.Value =(decimal) op;
            label7.Text = dns.pathtwo;
            btn_font.Text = myfont.Name;
            panel1.BackColor = fColor;
            panel2.BackColor = bgColor;
            num_FontSize.Value = (decimal)myfont.Size;

            //num_FontSize.Value = (decimal)myfont.Size;
            //richTextBox1.Font = myfont;
            //richTextBox1.BackColor = bgColor;
            refreshSetting();
        }
        void refreshSetting()
        {
            richTextBox1.Font = myfont;
            richTextBox1.ForeColor = fColor;
            richTextBox1.BackColor = bgColor;
            num_FontSize.Value = (decimal)myfont.Size;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            {
                fColor = cd.Color;
                refreshSetting();
                panel1.BackColor = fColor;

            }
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                bgColor = cd.Color;
                refreshSetting();
                panel2.BackColor = bgColor;

            }
        }
        /// <summary>
        /// save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            f.childForm.richTextBox1.Font = myfont;
            f.childForm.richTextBox1.ForeColor = fColor;
           f. childForm.label1.Font = myfont;
          f.  childForm.label1.ForeColor = fColor;

            f.BackColor = bgColor;
            //  childForm.richTextBox1.BackColor = this.BackColor;
            f.Opacity = op == 0 ? 0.01 : op / 100;
            f.dns.pathtwo = path;
            //MessageBox.Show(dno.s);
          //  f.writeSet();
            
        }

        

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            op = (double)numericUpDown1.Value;
        }

        private void btn_path_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = @"text|*.txt";
            sfd.ShowDialog();
            label7.Text = sfd.FileName;
            path = sfd.FileName;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
