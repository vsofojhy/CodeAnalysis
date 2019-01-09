using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeAnalysis
{
    public partial class reportGenerator : Form
    {
        public reportGenerator()
        {
            InitializeComponent();
        }
        static CodeAnalysis.LibClass.OpenCoverModel.GlobalModel golbalModel = new LibClass.OpenCoverModel.GlobalModel();
        string sJson = string.Empty;
        private void reportGenerator_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + LibClass.ConstValue.JSON_Global, System.Text.Encoding.UTF8))
            {
                sJson = sr.ReadToEnd();
            }
            golbalModel = Soholife.Common.Json.ConvertToJson.ParseFormJson<LibClass.OpenCoverModel.GlobalModel>(sJson);
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FileName = System.IO.Path.GetFileNameWithoutExtension(textBox1.Text);
            string SavePath = textBox2.Text + "\\" + FileName + "_Html";
            if (!System.IO.Directory.Exists(SavePath))
            {
                System.IO.Directory.CreateDirectory(SavePath);
            }
            else
            {
                DeleteFolder(SavePath);
            }

            string sCmd = "{0} -reports:{1} -targetdir:{2}";

            LibClass.OpenCover openCover = new LibClass.OpenCover();
            openCover.TransfEvent += AddMsg;
            string shell = string.Format(sCmd, golbalModel.ReportGenerator, textBox1.Text, SavePath);
            openCover.ThreadExec(shell);

            System.Diagnostics.Process.Start("explorer.exe", SavePath);
        }
        private void AddMsg(string msg)
        {
            textBox3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + textBox3.Text.Insert(0, msg + "\r\n");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*xml)|*.xml"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;//返回文件的完整路径            

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox2.Text = foldPath;
            }
        }

        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }

    }
}
