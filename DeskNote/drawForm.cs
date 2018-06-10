using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DeskNote
{
    public partial class drawForm : Form
    {
        Form1 f2;
        public drawForm(Form1 f)
        {
            InitializeComponent();
            f2 = f;
            this.BackColor = f.BackColor;
            this.TransparencyKey = this.BackColor;
            label1.Top = 3;
            label1.Left = -1;
            richTextBox1.Visible = false;

            
           
        }

        void trackBar1_Scroll(object sender, EventArgs e)
        {
         
        }
        /// <summary>  
        /// 边框阴影  
        /// </summary>  
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams createParams = base.CreateParams;
        //        createParams.ClassStyle |= 0x20000;
        //        return createParams;
        //    }
        //}

        

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
             richTextBox1.Visible = false;
            label1.Text = richTextBox1.Text;
            f2.isAnyChange = true;
        }

        
    }

}
