using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass.OpenCoverModel
{
    public class GlobalModel
    {
        public string OpenCover {get;set;}

        public string ReportGenerator { get; set; }

        public string ApplicationHost { get; set; }

        public string Register { get; set; }

        public GlobalModel()
        {
            this.OpenCover = "";
            this.ReportGenerator = "";
            this.ApplicationHost = "";
            this.Register = "administrator";
        }
    }
}
