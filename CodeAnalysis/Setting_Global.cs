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
    public partial class Setting_Global : Form
    {
        public delegate void LoadGlobalEvent(LibClass.OpenCoverModel.GlobalModel model);
        public event LoadGlobalEvent TransfEvent; 

        static LibClass.OpenCoverModel.GlobalModel model = new LibClass.OpenCoverModel.GlobalModel();
        static string sJson = string.Empty;
        public Setting_Global()
        {
            init();
            InitializeComponent();

            LoadModel();
        }

        static void init()
        {
            if (!File.Exists(LibClass.ConstValue.JSON_Global))
            {
                LibClass.OpenCoverModel.GlobalModel modelTemp = new LibClass.OpenCoverModel.GlobalModel();

                string sTempJson = Soholife.Common.Json.ConvertToJson.ObjectToJSON(modelTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_Global, sTempJson);
            }
            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + LibClass.ConstValue.JSON_Global, System.Text.Encoding.UTF8))
            {
                sJson = sr.ReadToEnd();
            }
            model = Soholife.Common.Json.ConvertToJson.ParseFormJson<LibClass.OpenCoverModel.GlobalModel>(sJson);
        }

        void LoadModel()
        {
            textBox1.Text = model.OpenCover;
            textBox2.Text = model.ReportGenerator;
            textBox3.Text = model.ApplicationHost;
            textBox4.Text = model.Register;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*exe)|*.exe"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;//返回文件的完整路径            

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*exe)|*.exe"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fileDialog.FileName;//返回文件的完整路径            

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            //C:\Users\vsofojhy\Documents\IISExpress\config\applicationhost.config
            fileDialog.Filter = "所有文件(*config)|*.config"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = fileDialog.FileName;//返回文件的完整路径         
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("OpenCover文件不存在", "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!File.Exists(textBox2.Text))
            {
                MessageBox.Show("ReportGenerator文件不存在", "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!File.Exists(textBox3.Text))
            {
                MessageBox.Show("ApplicationHost文件不存在", "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("用户名称为空", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            model.OpenCover = textBox1.Text;
            model.ReportGenerator = textBox2.Text;
            model.ApplicationHost = textBox3.Text;
            model.Register = textBox4.Text;
            string sJsonTemp = Soholife.Common.Json.ConvertToJson.ObjectToJSON(model);
            Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_Global, sJsonTemp);
            TransfEvent(model);
            init();
            LoadModel();

            MessageBox.Show("保存成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
