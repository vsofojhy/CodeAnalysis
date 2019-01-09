using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass.OpenCoverModel.JSON
{
    public class WebModel
    {
        public const string TARGET_DIR = "targetdir";

        public const string SITE = "site";

        public const string APPLICATION_HOST = "applicationhost";

        public const string REGISTER = "register";

        public const string OUTPUT = "output";

        public const string PORT = "port";

        //public const string OPENCOVER = "opencover";

        //public const string REPORT_GENERATOR = "ReportGenerator";


        public static List<JsonModel> GetList()
        {
            List<JsonModel> list = new List<JsonModel>();
            list.Add(new JsonModel() { Name = TARGET_DIR, Title = "网站目录", Value = "" });
            list.Add(new JsonModel() { Name = SITE, Title = "网站名称", Value = "" });
            list.Add(new JsonModel() { Name = APPLICATION_HOST, Title = "站点配置文件", Value = "" });
            list.Add(new JsonModel() { Name = REGISTER, Title = "系统用户名称", Value = "" });
            list.Add(new JsonModel() { Name = OUTPUT, Title = "结果数据路径", Value = "" });
            list.Add(new JsonModel() { Name = PORT, Title = "网站端口", Value = "" });
            //list.Add(new JsonModel() { Name = OPENCOVER, Title = "Opencover物理路径", Value = "" });
            //list.Add(new JsonModel() { Name = REPORT_GENERATOR, Title = "ReportGenerator物理路径", Value = "" });

            return list;
        }
    }
}
