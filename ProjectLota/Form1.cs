using ProjectLota.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLota
{
    public partial class LotaJammer : Form
    {
        public string cFile = "config.lota";
        public LotaJammer()
        {
            InitializeComponent();
        }

        List<string> items = new List<string>();
        List<string> stwingawway = new List<string>();
        private void LotaJammer_Load(object sender, EventArgs e)
        {
            //Thread thrd = new Thread(ListProcesses) { IsBackground = true };
            LoadSettings();
            ListProcesses();
            items.AddRange(stwingawway);
            label1.Text = "";
            foreach (string str in items)
            {
                listBox1.Items.Add(str);
            }
        }

        Process[] processCollection;
        private void ListProcesses()
        {
            processCollection = Process.GetProcesses();
            foreach (Process p in processCollection)
            {
                listBox1.Items.Add(p.ProcessName);
                stwingawway.Add(p.ProcessName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text=null;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (string str in items)
            {
                if (str.StartsWith(textBox1.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    listBox1.Items.Add(str);
                }
            }
        }
        public List<string> selectedItemsA=new List<string>();
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //usual stuff
            var selectedItem = listBox1.SelectedItem;
            selectedItemsA.Add(selectedItem.ToString());
            listBox2.Items.Add(selectedItem);
            //listBox2.Items.Clear();
            SaveSettings();
        }
        public void SaveSettings(){
            List<string> s = new List<string>();
            foreach (string asd in listBox2.Items)
            {
                s.Add(asd);
            }
            if (!File.Exists(cFile))
            {
                var file=File.Create(cFile);
                file.Close();
            }
            File.WriteAllLines(cFile, s);
        }
        public void LoadSettings()
        {
            List<string> s = new List<string>();
            string[] ss;
            if (File.Exists(cFile))
            {
                var sss = File.ReadAllLines(cFile);
                s.Clear();
                foreach (string ssss in sss){
                    s.Add(ssss);
                }
                ss = s.ToArray();
                listBox2.Items.Clear();
                foreach (string sssss in ss){
                    listBox2.Items.Add(sssss);
                }
                selectedItemsA=s;
            }

        }
        //Kill sellected process
        public bool doLoopKill=false;
        private void button1_Click(object sender, EventArgs e)
        {
            doLoopKill = !doLoopKill;
            //Change button color based on doLoopKill value
            if (doLoopKill)
            {
                button1.ForeColor = Color.Green;
                cIcon(Color.Green);
                Bitmap sB = new Bitmap(pictureBox1.Image);
                this.Icon = Icon.FromHandle(sB.GetHicon());
                sB.Dispose();
            }
            else
            {
                button1.ForeColor = Color.Red;
                cIcon(Color.Red);
                Bitmap sB = new Bitmap(pictureBox1.Image);
                this.Icon = Icon.FromHandle(sB.GetHicon());
                sB.Dispose();
            }
            Thread thrd = new Thread(noFreezeKillLoop) { IsBackground = true };
            if (doLoopKill){
                if (selectedItemsA.Count < 0)
                {
                    label1.Text = "Processes are getting jammed :))";
                }
                else
                {
                    label1.Text = "Process is getting jammed :)";
                }
                thrd.Start();
            }
            else{
                thrd.Abort();
            }
        }
        public void noFreezeKillLoop(){
            //Kill sellected process
            while (doLoopKill)
            {
                Process[] pp;
                pp = Process.GetProcesses();
                foreach (Process p in pp)
                {
                    foreach (string Name in selectedItemsA){
                        if (p.ProcessName == Name)
                        {
                            //Debug.WriteLine(p.ProcessName, p.Id);
                            p.Kill();
                            //Invoke(new Action(() =>
                            //{

                            //}));
                        }
                    }
                }
                Task.Delay(100).Wait();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Set window icon to grey (Default)
            cIcon(Color.Gray);
            Bitmap sB = new Bitmap(pictureBox1.Image);
            this.Icon = Icon.FromHandle(sB.GetHicon());
            sB.Dispose();
            //Set button1.ForeGround color to black
            button1.ForeColor = Color.Black;
            doLoopKill = false;
            label1.Text = "";
            //Reload ListBox1
            listBox1.Items.Clear();
            Process[] pr;
            pr = Process.GetProcesses();
            List<string> s = new List<string>();
            foreach (Process word in pr)
            {
                listBox1.Items.Add(word.ProcessName);
                s.Add(word.ProcessName);
            }

            //Reload Filters
            items.Clear();
            items.AddRange(s);
            foreach (string str in items)
            {
                listBox1.Items.Add(str);
            }
            listBox1.Items.Clear();

            foreach (string str in items)
            {
                if (str.StartsWith(textBox1.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    listBox1.Items.Add(str);
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem!=null){
                listBox2.Items.Remove(listBox2.SelectedItem);
                listBox2.SelectedItem = null;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            File.WriteAllText(cFile, "");
        }
        PictureBox pictureBox1 = new PictureBox();
        public void cIcon(Color brush)
        {
            pictureBox1.Size = new Size(400, 400);
            Bitmap flag = new Bitmap(400, 400);
            Graphics flagGraphics = Graphics.FromImage(flag);
            Brush clr = new SolidBrush(brush);
            flagGraphics.FillRectangle(clr, 0, 0, 400, 400);
            pictureBox1.Image = flag;
        }
    }
}