using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DeskNote
{
   public class DesktopNoteSetting
    {
        public int wintop = 150;
        public int winleft = 300;
        public int winwidth = 600;
        public int winheight = 400;
        public int fSize = 18;
        public string fontname = "Arial";
        public int fStyle = 0;
        public string fColor = "#535353";
        public string bgColor = "#CBC2DA";
        public double op = 85;
        public string autosave = "f";
        public string pathtwo = @"D:\desknote.txt";
        public string staytop = "f";
        public string autoremote = "f";
        public string remotepath = "";

        public StringBuilder sb = new StringBuilder();

        public void readConfig()
        {
            //将config.ini属性填入文本框
            if (File.Exists(Application.StartupPath + "/config.ini"))
            {
                try
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "/config.ini");
                    wintop = int.Parse(sr.ReadLine());
                    winleft = int.Parse(sr.ReadLine());
                    winwidth = int.Parse(sr.ReadLine());
                    winheight = int.Parse(sr.ReadLine());
                    fSize = int.Parse(sr.ReadLine());
                    fontname = sr.ReadLine();
                    fStyle = int.Parse(sr.ReadLine());
                    fColor = sr.ReadLine();
                    bgColor = sr.ReadLine();
                    op = double.Parse(sr.ReadLine());
                    autosave = sr.ReadLine();
                    pathtwo = sr.ReadLine();
                    staytop = sr.ReadLine();
                    autoremote = sr.ReadLine();
                    remotepath = sr.ReadLine();
                    sr.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("error: " + e + ". Please rename the config.bak to config.ini in the software path, and restart the software.");
                }

            }
            else
            {
                //  MessageBox.Show("The config file broken. Please rename the config.bak to config.ini in the software path, and restart the software");
            }


            if(autoremote =="t" && File.Exists(remotepath))
            {
                //read remote
                StreamReader sr = new StreamReader(remotepath, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                   
                    sb.Append(line.ToString() + "\r");
                }
                sr.Close();
            }
            else
            {
                if (autoremote == "t")
                    if (!File.Exists(remotepath))
                        MessageBox.Show("Remote path not exist!Please set remote path again, and check the AutoRemoteSYNC");
                //read local
                autoremote = "f";
                if (File.Exists(Application.StartupPath + "/desknote.txt"))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "/desknote.txt", Encoding.Default);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //  sb.Append(line.ToString() + "\r\n");
                        sb.Append(line.ToString() + "\r");
                    }
                    sr.Close();
                }
                else
                {
                    sb.Append("Click once into edit mode.\rDouble click to end edit.\rOption can change by right click the notice icon. \rText will be autosave in any changes,\rand reload again when you restart the program.");
                    // MessageBox.Show("");
                }
            }


           



        }
        public void writeConfig()
        {
            autosave = (autosave == "" ? "f" : autosave);
            staytop =( staytop == "" ? "f" : staytop);
            autoremote = autoremote == "" ? "f" : autoremote;

            string s = String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n{8}\r\n{9}\r\n{10}\r\n{11}\r\n{12}\r\n{13}\r\n{14}\r\n", wintop.ToString(), winleft.ToString(), winwidth.ToString(), winheight.ToString(), fSize.ToString(), fontname, fStyle, fColor, bgColor, op, autosave, pathtwo, staytop,autoremote,remotepath);
            File.WriteAllText(Application.StartupPath + "/config.ini", s, Encoding.ASCII);//保存到一个文件里（续写，覆盖用File.WriteAllText）

            File.WriteAllText(Application.StartupPath + "/desknote.txt", sb.ToString(), Encoding.ASCII);//保存到一个文件里（续写，覆盖用File.WriteAllText）
            File.WriteAllText(pathtwo, sb.ToString(), Encoding.ASCII);//保存到一个文件里（续写，覆盖用File.WriteAllText）
            if (autoremote == "t")
            {
                if (File.Exists(remotepath))
                    File.WriteAllText(remotepath, sb.ToString(), Encoding.ASCII);
                else if(remotepath !="")
                {
                    try
                    {
                        File.WriteAllText(remotepath, sb.ToString(), Encoding.ASCII);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Remote path not exist" + e.ToString());
                    }
                }
            }
            
        }

    }
}
