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
    public partial class Setting_EXE : Form
    {
        static List<CodeAnalysis.LibClass.OpenCoverModel.ExeModel> list = new List<LibClass.OpenCoverModel.ExeModel>();
        static string sJson = string.Empty;
        public Setting_EXE()
        {
            init();
            InitializeComponent();
            LoadListBox(); 
        }

        /// <summary>
        /// 初始化加载配置
        /// </summary>
        static void init()
        {
            if (!File.Exists(LibClass.ConstValue.JSON_EXE))
            {
                List<CodeAnalysis.LibClass.OpenCoverModel.ExeModel> listTemp = new List<LibClass.OpenCoverModel.ExeModel>();
                listTemp.Add(new LibClass.OpenCoverModel.ExeModel());
                string sTempJson = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_EXE, sTempJson);
            }
            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + LibClass.ConstValue.JSON_EXE, System.Text.Encoding.UTF8))
            {
                sJson = sr.ReadToEnd();
            }
            list = Soholife.Common.Json.ConvertToJson.ParseFormJson<List<LibClass.OpenCoverModel.ExeModel>>(sJson);
        }

        void LoadListBox()
        {
            listBox1.Items.Clear();
            foreach (CodeAnalysis.LibClass.OpenCoverModel.ExeModel item in list)
            {
                if (!string.IsNullOrEmpty(item.FileName))
                    listBox1.Items.Add(item.FileName);
            }
            listBox1.Items.Add("增加+");
            button3.Text = "保存";
        }

        private void Setting_Web_Load(object sender, EventArgs e)
        {


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
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox2.Text = foldPath;
                //MessageBox.Show("已选择文件夹:" + foldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public event  LoadExeModel ExeModelEvent; 
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
          
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("选择的执行文件不存在", "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Directory.Exists(textBox2.Text))
            {
                MessageBox.Show("文件保存路径不存在", "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string Target = textBox1.Text;
            string TargetDir = Directory.GetParent(Target).FullName;
            string FileName = Path.GetFileName(Target);
            string OutPut = textBox2.Text;//+ "\\" + Path.GetFileNameWithoutExtension(Target) + ".xml";


            List<CodeAnalysis.LibClass.OpenCoverModel.ExeModel> listTemp = new List<LibClass.OpenCoverModel.ExeModel>();

            List<CodeAnalysis.LibClass.OpenCoverModel.ExeModel> listCopy = new List<LibClass.OpenCoverModel.ExeModel>();
            foreach (CodeAnalysis.LibClass.OpenCoverModel.ExeModel item in list)
            {
                if (item.FileName.ToUpper() != FileName.ToUpper())
                {
                    listCopy.Add(item);
                }
            }
            foreach (CodeAnalysis.LibClass.OpenCoverModel.ExeModel item in listCopy)
            {
                if (!string.IsNullOrEmpty(item.TargetDir))
                {
                    listTemp.Add(item);
                }
            }

            listTemp.Add(new CodeAnalysis.LibClass.OpenCoverModel.ExeModel() { TargetDir = TargetDir, Target = Target, OutPut = OutPut, FileName = FileName });
            string sJsonTemp = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
            Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_EXE, sJsonTemp);

            ExeModelEvent(listTemp);

            init();
            LoadListBox();
            listBox1.SetSelected(listBox1.Items.Count - 2, true);
            MessageBox.Show("保存成功", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CodeAnalysis.LibClass.OpenCoverModel.ExeModel model = new LibClass.OpenCoverModel.ExeModel();
            foreach (CodeAnalysis.LibClass.OpenCoverModel.ExeModel item in list)
            {
                if (item.FileName == listBox1.SelectedItem.ToString())
                {
                    model = item;
                }
            }
            textBox1.Text = model.Target;
            textBox2.Text = model.OutPut;
            if (listBox1.SelectedItem.ToString() == "增加+")
            {
                button3.Text = "保存";
            }
            else
            {
                button3.Text = "修改";
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
                List<LibClass.OpenCoverModel.ExeModel> listTemp = new List<LibClass.OpenCoverModel.ExeModel>();
                foreach (LibClass.OpenCoverModel.ExeModel item in list)
                {
                    if (item.FileName.ToUpper() != listBox1.SelectedItem.ToString().ToUpper())
                    {
                        listTemp.Add(item);
                    }
                }
                string sJsonTemp = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_EXE, sJsonTemp);
                ExeModelEvent(listTemp);
                init();
                LoadListBox();
            }
        }

      


    }
}
