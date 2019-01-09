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
    public partial class Main : Form
    {
        string TypeModel = "web";
        static List<CodeAnalysis.LibClass.OpenCoverModel.WebModel> listWeb = new List<LibClass.OpenCoverModel.WebModel>();
        static List<CodeAnalysis.LibClass.OpenCoverModel.ExeModel> listExe = new List<LibClass.OpenCoverModel.ExeModel>();
        static CodeAnalysis.LibClass.OpenCoverModel.GlobalModel golbalModel = new LibClass.OpenCoverModel.GlobalModel();

        static CodeAnalysis.LibClass.OpenCoverModel.WebModel ModelWeb = new LibClass.OpenCoverModel.WebModel();
        static CodeAnalysis.LibClass.OpenCoverModel.ExeModel ModelExe = new LibClass.OpenCoverModel.ExeModel();

        string sJson = string.Empty;
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            AddMsg("已启动程序");

            string sJson = string.Empty;
            #region 加载全局配置

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
            golbalModel = Soholife.Common.Json.ConvertToJson.ParseFormJson<LibClass.OpenCoverModel.GlobalModel>(sJson);

            if (!File.Exists(golbalModel.OpenCover))
            {
                MessageBox.Show("请设置 Opencover路径");
                Setting_Global frm = new Setting_Global();
                frm.TransfEvent += frm_TransfEvent;
                frm.ShowDialog();

            }
            if (!File.Exists(golbalModel.ReportGenerator))
            {
                MessageBox.Show("请设置ReportGenerator路径");
                Setting_Global frm = new Setting_Global();
                frm.TransfEvent += frm_TransfEvent;
                frm.ShowDialog();

            }
            if (!File.Exists(golbalModel.ApplicationHost))
            {
                MessageBox.Show("请设置ApplicationHost路径");
                Setting_Global frm = new Setting_Global();
                frm.TransfEvent += frm_TransfEvent;
                frm.ShowDialog();

            }
            #endregion
        }

        void frm_TransfEvent(LibClass.OpenCoverModel.GlobalModel model)
        {
            golbalModel = model;
            AddMsg("已重新加载全局配置信息");
        }

        #region 菜单栏按钮

        /// <summary>
        ///  执行文件配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.SelectedItem = "";
            Setting_EXE frm = new Setting_EXE();
            frm.ExeModelEvent += LoadListExe;
            frm.ShowDialog();
        }
        void LoadListExe(List<LibClass.OpenCoverModel.ExeModel> list)
        {
            listExe = list;
           
            foreach (LibClass.OpenCoverModel.ExeModel item in listExe)
            {
                if (item.FileName.ToUpper() == ModelExe.FileName.ToUpper())
                {
                    ModelExe = item;
                }
            }
            LoadExe();
            LoadListBoxByExe(ModelExe.FileName);

        }

        /// <summary>
        /// 站点配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting_Web frm = new Setting_Web();
            frm.WebModelEvent += LoadListWeb;
            frm.ShowDialog();
        }
        void LoadListWeb(List<LibClass.OpenCoverModel.WebModel> list)
        {
            listWeb = list;
            foreach (LibClass.OpenCoverModel.WebModel item in listWeb)
            {
                if (item.Site.ToUpper() == ModelWeb.Site.ToUpper())
                {
                    ModelWeb = item;
                }
            }
            LoadWeb();
            LoadListBoxByWeb(ModelWeb.Site);
        }
        /// <summary>
        /// 全局配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoladToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting_Global frm = new Setting_Global();
            frm.TransfEvent += frm_TransfEvent;
            frm.ShowDialog();
        }

        private void web测试说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_Web frm = new About_Web();
            frm.ShowDialog();
        }

        private void exe测试说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("暂时没有说明", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #endregion


        void LoadWeb()
        {

            label6.Visible = true;
            label1.Text = "网站Bin目录:";
            label2.Text = "站点名称:";
            label3.Text = "输出路径:";
            label4.Text = "端口:";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            label4.Visible = true;
            textBox4.Visible = true;

            listBox1.Items.Clear();
            if (!File.Exists(LibClass.ConstValue.JSON_WEB))
            {
                List<CodeAnalysis.LibClass.OpenCoverModel.WebModel> listTemp = new List<LibClass.OpenCoverModel.WebModel>();
                listTemp.Add(new LibClass.OpenCoverModel.WebModel());
                string sTempJson = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_WEB, sTempJson);
                AddMsg("暂未配置web测试相关信息");
            }
            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + LibClass.ConstValue.JSON_WEB, System.Text.Encoding.UTF8))
            {
                sJson = sr.ReadToEnd();
            }
            listWeb = Soholife.Common.Json.ConvertToJson.ParseFormJson<List<LibClass.OpenCoverModel.WebModel>>(sJson);
            AddMsg("已加载web测试模式");

            foreach (LibClass.OpenCoverModel.WebModel item in listWeb)
            {
                if (!string.IsNullOrEmpty(item.Site))
                {
                    listBox1.Items.Add(item.Site);
                }
            }
            groupBox1.Text = "WEB模式";
            groupBox1.ForeColor = Color.BlueViolet;
        }
        void LoadExe()
        {
            label6.Visible = false;
            label1.Text = "应用根目录:";
            label2.Text = "输出路径:";
            label3.Text = "文件名称:";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label4.Visible = false;
            textBox4.Visible = false;
            listBox1.Items.Clear();
            if (!File.Exists(LibClass.ConstValue.JSON_EXE))
            {
                List<CodeAnalysis.LibClass.OpenCoverModel.ExeModel> listTemp = new List<LibClass.OpenCoverModel.ExeModel>();
                listTemp.Add(new LibClass.OpenCoverModel.ExeModel());
                string sTempJson = Soholife.Common.Json.ConvertToJson.ObjectToJSON(listTemp);
                Soholife.Common.IO.FileUtils.WriteFile(LibClass.ConstValue.JSON_EXE, sTempJson);
                AddMsg("暂未配置exe测试相关信息");
            }

            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + LibClass.ConstValue.JSON_EXE, System.Text.Encoding.UTF8))
            {
                sJson = sr.ReadToEnd();
            }
            listExe = Soholife.Common.Json.ConvertToJson.ParseFormJson<List<LibClass.OpenCoverModel.ExeModel>>(sJson);
            AddMsg("已加载exe测试模式");
            foreach (LibClass.OpenCoverModel.ExeModel item in listExe)
            {
                if (!string.IsNullOrEmpty(item.FileName))
                {
                    listBox1.Items.Add(item.FileName);
                }
            }
            groupBox1.Text = "EXE模式";
            groupBox1.ForeColor = Color.Red;
        }

        void LoadListBoxByWeb(string site)
        {

            foreach (LibClass.OpenCoverModel.WebModel item in listWeb)
            {
                if (item.Site.ToUpper() == site.ToUpper())
                {
                    label1.Text = "网站Bin目录:";
                    label2.Text = "站点名称:";
                    label3.Text = "输出路径:";
                    label4.Text = "端口:";
                    textBox1.Text = item.TargetDir;
                    textBox2.Text = item.Site;
                    textBox3.Text = item.OutPut;
                    textBox4.Text = item.Port;
                    ModelWeb = item;
                }
            }
        }
        void LoadListBoxByExe(string sFileName)
        {

            foreach (LibClass.OpenCoverModel.ExeModel item in listExe)
            {
                if (item.FileName.ToUpper() == sFileName.ToUpper())
                {
                    label1.Text = "应用根目录:";
                    label2.Text = "输出路径:";
                    label3.Text = "文件名称:";
                    // label4.Text = "文件名称:";
                    textBox1.Text = item.TargetDir;
                    textBox2.Text = item.OutPut;
                    textBox3.Text = item.FileName;
                    //textBox4.Text = item.FileName;
                    label4.Visible = false;
                    textBox4.Visible = false;

                    ModelExe = item;
                }
            }
        }



        private void Main_Load(object sender, EventArgs e)
        {
            switch (TypeModel)
            {
                case "web":
                    LoadWeb();
                    break;
                case "exe":
                    LoadExe();
                    break;
            }
        }



        private void AddMsg(string msg)
        {
            txtMsg.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + txtMsg.Text.Insert(0, msg + "\r\n");
        }

        private void web测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            LoadWeb();
        }

        private void exe测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            LoadExe();
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string sItemStr = listBox1.SelectedItem.ToString();
            switch (groupBox1.Text)
            {
                case "WEB模式":
                    LoadListBoxByWeb(sItemStr);
                    break;
                case "EXE模式":
                    LoadListBoxByExe(sItemStr);
                    break;
            }
        }


        string sXmlPath = "";
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            sXmlPath = "";
            LibClass.OpenCover openCover = new LibClass.OpenCover();
            openCover.TransfEvent += AddMsg;
            openCover.XMLEvent += SetXmlPath;

            switch (groupBox1.Text)
            {
                case "WEB模式":
                    openCover.Exec(ModelWeb, golbalModel);
                    break;
                case "EXE模式":
                    openCover.Exec(ModelExe, golbalModel);
                    break;
            }

        }

        /// <summary>
        /// 生成html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sXmlPath))
            {
                MessageBox.Show("没有执行");
                return;
            }
            LibClass.OpenCover openCover = new LibClass.OpenCover();
            openCover.TransfEvent += AddMsg;
            string sCmd = "{0} -reports:{1} -targetdir:{2}";
            string sHtmlPaht=Directory.GetParent(sXmlPath).FullName;
            if (!Directory.Exists(sHtmlPaht + "\\html"))
            {
                Directory.CreateDirectory(sHtmlPaht + "\\html");
            }
            string shell = string.Format(sCmd, golbalModel.ReportGenerator, sXmlPath, sHtmlPaht+"\\html");
            openCover.ThreadExec(shell);

            System.Diagnostics.Process.Start("explorer.exe", sHtmlPaht + "\\html");
        }

        void SetXmlPath(string sPath)
        {
            sXmlPath = sPath;
        }

        private void reportGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reportGenerator frm = new reportGenerator();
            frm.ShowDialog();
        }

    }
}
