using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass.OpenCoverModel.JSON
{
    public class ExeModel
    {

        /// <summary>
        /// 执行文件的完整路径
        /// </summary>
        public const string TARGET = "target";

        /// <summary>
        /// 覆盖率输出文件
        /// </summary>
        public const string OUTPUT = "output";


        ///// <summary>
        ///// OpenCover的根目录路径
        ///// </summary>
        //public const string OPENCOVER = "opencover";

        //public const string REPORT_GENERATOR = "ReportGenerator";


        public static List<JsonModel> GetList()
        {
            List<JsonModel> list = new List<JsonModel>();
            list.Add(new JsonModel() { Name = TARGET, Value = "", Title = "执行文件的完整路径" });
            list.Add(new JsonModel() { Name = TARGET, Value = "", Title = "结果数据路径" });

            return list;
        }
    }
}
