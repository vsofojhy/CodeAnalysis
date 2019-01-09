using CodeAnalysis.LibClass;
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

    public partial class Setting_Web : Form
    {
        static List<CodeAnalysis.LibClass.OpenCoverModel.WebModel> list = new List<LibClass.OpenCoverModel.WebModel>();
        static string sJson = string.Empty;
        public Setting_Web()
        {
            init();
            InitializeComponent();
            LoadListBox();
        }

        static void init()
        {
            if (!File.Exists(LibClass.ConstValue.JSON_WEB))
            {
                List<CodeAnalysis.LibClass.OpenCoverModel.WebModel> listTemp = new List<LibClass.OpenCoverModel.WebModel>();
                listTemp.Add(new LibClass.OpenCoverModel.WebModel());
                string sTempJson = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_WEB, sTempJson);
            }
            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + LibClass.ConstValue.JSON_WEB, System.Text.Encoding.UTF8))
            {
                sJson = sr.ReadToEnd();
            }
            list = Soholife.Common.Json.ConvertToJson.ParseFormJson<List<LibClass.OpenCoverModel.WebModel>>(sJson);
        }

        void LoadListBox()
        {
            listBox1.Items.Clear();
            foreach (CodeAnalysis.LibClass.OpenCoverModel.WebModel item in list)
            {
                if (!string.IsNullOrEmpty(item.Site))
                    listBox1.Items.Add(item.Site);
            }
            listBox1.Items.Add("增加+");
            btnSave.Text = "保存";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtTargetDir.Text = foldPath;
                //MessageBox.Show("已选择文件夹:" + foldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtOutPut.Text = foldPath;
            }
        }

        public event LoadWebModel WebModelEvent;
        private void btnSave_Click(object sender, EventArgs e)
        {
            string Targetdir = txtTargetDir.Text;
            string Site = txtSite.Text;
            string Output = txtOutPut.Text;
            int Port = Soholife.Common.CommonUtils.GetIntValue(txtPort.Text);
            if (!Directory.Exists(Targetdir))
            {
                MessageBox.Show("网站路径不存在", "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Directory.Exists(Output))
            {
                MessageBox.Show("文件保存路径不存在", "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Site))
            {
                MessageBox.Show("网站名称不能为空", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Port == 0)
            {
                MessageBox.Show("请填写正确的端口号", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<CodeAnalysis.LibClass.OpenCoverModel.WebModel> listTemp = new List<LibClass.OpenCoverModel.WebModel>();

            List<CodeAnalysis.LibClass.OpenCoverModel.WebModel> listCopy = new List<LibClass.OpenCoverModel.WebModel>();
            foreach (CodeAnalysis.LibClass.OpenCoverModel.WebModel item in list)
            {
                if (item.Site.ToUpper() != Site.ToUpper())
                {
                    listCopy.Add(item);
                }
            }

            foreach (CodeAnalysis.LibClass.OpenCoverModel.WebModel item in listCopy)
            {
                if (!string.IsNullOrEmpty(item.TargetDir))
                {
                    listTemp.Add(item);
                }
            }

            listTemp.Add(new CodeAnalysis.LibClass.OpenCoverModel.WebModel() { OutPut = Output, Port = Port.ToString(), Site = Site, TargetDir = Targetdir });
            string sJsonTemp = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
            Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_WEB, sJsonTemp);
            WebModelEvent(listTemp);
            init();
            LoadListBox();
            listBox1.SetSelected(listBox1.Items.Count - 2, true);
            MessageBox.Show("保存成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CodeAnalysis.LibClass.OpenCoverModel.WebModel model = new LibClass.OpenCoverModel.WebModel();
            foreach (CodeAnalysis.LibClass.OpenCoverModel.WebModel item in list)
            {
                if (item.Site == listBox1.SelectedItem.ToString())
                {
                    model = item;
                }
            }
            txtOutPut.Text = model.OutPut;
            txtPort.Text = model.Port;
            txtSite.Text = model.Site;
            txtTargetDir.Text = model.TargetDir;

            if (listBox1.SelectedItem.ToString() == "增加+")
            {
                btnSave.Text = "保存";
            }
            else
            {
                btnSave.Text = "修改";
            }
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int posindex = listBox1.IndexFromPoint(new Point(e.X, e.Y));
                listBox1.ContextMenuStrip = null;
                if (posindex >= 0 && posindex < listBox1.Items.Count)
                {
                    listBox1.SelectedIndex = posindex;
                    contextMenuStrip1.Show(listBox1, new Point(e.X, e.Y));
                }
            }
            listBox1.Refresh();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<LibClass.OpenCoverModel.WebModel> listTemp = new List<LibClass.OpenCoverModel.WebModel>();
                foreach (LibClass.OpenCoverModel.WebModel item in list)
                {
                    if (item.Site.ToUpper() != listBox1.SelectedItem.ToString().ToUpper())
                    {
                        listTemp.Add(item);
                    }
                }
                string sJsonTemp = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_WEB, sJsonTemp);
                WebModelEvent(listTemp);
                init();
                LoadListBox();
            }
        }


    }
}
