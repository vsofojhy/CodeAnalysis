using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass.OpenCoverModel
{
    public class ExeModel  
    {
        public string TargetDir { get; set; }
        public string Target { get; set; }
        public string OutPut { get; set; }

        public string FileName { get; set; }

        public ExeModel()
        {
            this.TargetDir = "";
            this.Target = "";
            this.OutPut = "";
            this.FileName = "";
        }
    }
}
