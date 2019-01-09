using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass.OpenCoverModel
{
    public class WebModel 
    {
        public string TargetDir { get; set; }

        public string Site { get; set; } 
        public string OutPut { get; set; }

        //public string ApplicationHost { get; set; }

        //public string Register { get; set; }
        public string Port { get; set; }

        public WebModel()
        {
            this.TargetDir = "";
            this.Site = "";
            this.OutPut = "";
            //this.ApplicationHost = "";
            //this.Register = "";
            this.Port = "";
        }

    }
}
