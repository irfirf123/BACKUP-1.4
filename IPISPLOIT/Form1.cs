using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KrnlAPI;
using Microsoft.Win32;
using System.Net;
using IRISAPI;
namespace IPISPLOIT

{
    public partial class IPISPLOIT : Form
    {
        IRISAPI.ExploitAPI exploitAPI = new IRISAPI.ExploitAPI();
        public IPISPLOIT()
        {
            InitializeComponent();
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "Scripts")))
                Directory.CreateDirectory(Path.Combine(currentPath, "Scripts"));
            label4.Text = ("Not injected");
            label4.ForeColor = Color.Red;
        }
        Point lastPoint;


        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e) // Сохранить
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(webBrowser1.Text);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e) // Инжект
        {
            if (comboBox1.SelectedIndex == 1)
            {
                SELECTDLLERROR openform = new SELECTDLLERROR();
                openform.Show();
            }
                if (comboBox1.SelectedIndex == 0)
                {
                    MainAPI.Inject(); // KRNL
                }
                if (comboBox1.SelectedIndex == 2) // Hovac
                {
                exploitAPI.LaunchExploit();
                }
        }


        private void button1_Click_1(object sender, EventArgs e) // Экзекьют
        {
            if (comboBox1.SelectedIndex == 1)
            {
                SELECTDLLERROR openform = new SELECTDLLERROR();
                openform.Show(); //WRD
            }
            if (comboBox1.SelectedIndex == 0)
            {
                    HtmlDocument document = webBrowser1.Document;
                    string scriptName = "GetText";
                    object[] args = new string[0];
                    object obj = document.InvokeScript(scriptName, args);
                    string script = obj.ToString();
                    MainAPI.Execute(script); // KRNL
            }
            if (comboBox1.SelectedIndex == 2)
            {
                            HtmlDocument document = webBrowser1.Document;
                            string scriptName = "GetText";
                            object[] args = new string[0];
                            object obj = document.InvokeScript(scriptName, args);
                            string script = obj.ToString();
                            exploitAPI.SendLuaScript(script);
            }
        }

        private void button2_Click_1(object sender, EventArgs e) // Очистить текстбокс
        {
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                ""
            });
        }

        private void button4_Click_1(object sender, EventArgs e) // Открыть файл
        {
            if (Functions.openfiledialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    string MainText = File.ReadAllText(Functions.openfiledialog.FileName);
                    webBrowser1.Document.InvokeScript("SetText", new object[]
                    {
                          MainText
                    });

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void IPISPLOIT_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void IPISPLOIT_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();//Clear Items in the LuaScriptList
            Functions.PopulateListBox(listBox1, "./Scripts", "*.txt");
            Functions.PopulateListBox(listBox1, "./Scripts", "*.lua");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string aboba = File.ReadAllText($"./Scripts/{listBox1.SelectedItem}");
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                          aboba
            });
        }

        WebClient wc = new WebClient();
        private string defPath = Application.StartupPath + "//Monaco//";

        private void addIntel(string label, string kind, string detail, string insertText)
        {
            string text = "\"" + label + "\"";
            string text2 = "\"" + kind + "\"";
            string text3 = "\"" + detail + "\"";
            string text4 = "\"" + insertText + "\"";
            webBrowser1.Document.InvokeScript("AddIntellisense", new object[]
            {
                label,
                kind,
                detail,
                insertText
            });
        }

        private void addGlobalF()
        {
            string[] array = File.ReadAllLines(this.defPath + "//globalf.txt");
            foreach (string text in array)
            {
                bool flag = text.Contains(':');
                if (flag)
                {
                    this.addIntel(text, "Function", text, text.Substring(1));
                }
                else
                {
                    this.addIntel(text, "Function", text, text);
                }
            }
        }

        private void addGlobalV()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalv.txt"))
            {
                this.addIntel(text, "Variable", text, text);
            }
        }

        private void addGlobalNS()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalns.txt"))
            {
                this.addIntel(text, "Class", text, text);
            }
        }

        private void addMath()
        {
            foreach (string text in File.ReadLines(this.defPath + "//classfunc.txt"))
            {
                this.addIntel(text, "Method", text, text);
            }
        }

        private void addBase()
        {
            foreach (string text in File.ReadLines(this.defPath + "//base.txt"))
            {
                this.addIntel(text, "Keyword", text, text);
            }
        }
        private async void IPISPLOIT_Load(object sender, EventArgs e)
        {

            comboBox1.SelectedIndex = 2;
            listBox1.Items.Clear();//Clear Items in the LuaScriptList
            Functions.PopulateListBox(listBox1, "./Scripts", "*.txt");
            Functions.PopulateListBox(listBox1, "./Scripts", "*.lua");
            WebClient wc = new WebClient();
            wc.Proxy = null;
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                bool flag2 = registryKey.GetValue(friendlyName) == null;
                if (flag2)
                {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
                registryKey = null;
                friendlyName = null;
            }
            catch (Exception)
            {
            }
            webBrowser1.Url = new Uri(string.Format("file:///{0}/Monaco/Monaco.html", Directory.GetCurrentDirectory()));
            await Task.Delay(500);
            webBrowser1.Document.InvokeScript("SetTheme", new string[]
            {
                   "Dark"
                   /*
                    There are 2 Themes Dark and Light
                   */
            });
            addBase();
            addMath();
            addGlobalNS();
            addGlobalV();
            addGlobalF();
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                 "-- made by f3m60y"
            });
        }


        private void button9_Click(object sender, EventArgs e)
        {
            Options openform = new Options();
            openform.Show();
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.Left += e.X - lastPoint.X;
                    this.Top += e.Y - lastPoint.Y;
                }
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
             bool status = ExploitAPI.isAPIAttached();
            if (status == false)
              label4.Text = ("Not injected");
               label4.ForeColor = Color.Red;
            
            if (status == true)
                label4.Text = ("Injected");
                label4.ForeColor = Color.Lime;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}