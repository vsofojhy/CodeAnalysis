using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass
{
    public delegate void EventShowMsg(string sMsg);

    public delegate void EventXML(string xmlPath);


    public delegate void LoadWebModel(List<LibClass.OpenCoverModel.WebModel> list);

    public delegate void LoadExeModel(List<LibClass.OpenCoverModel.ExeModel> list);

}
