using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodeAnalysis.LibClass
{
    public class OpenCover : BaseClass.IOpenCover
    {
        public event EventShowMsg TransfEvent;
        public event EventXML XMLEvent;

        string sExecShell = "{0} -register -target:\"{1}\" -output:{2} -targetdir:\"{3}\"";
        public void Exec(OpenCoverModel.ExeModel model, LibClass.OpenCoverModel.GlobalModel globalModel)
        {
            TransfEvent("EXE模式，执行文件" + model.FileName);
            string fileName = Path.GetFileNameWithoutExtension(model.Target);
            string sOutput = model.OutPut + "\\" + fileName;
            XMLEvent(sOutput + "\\" + fileName + ".xml");
            if (!Directory.Exists(sOutput))
            {
                Directory.CreateDirectory(sOutput);
            }
            string sShell = string.Format(sExecShell, globalModel.OpenCover, model.Target, sOutput +"\\"+ fileName + ".xml", model.TargetDir);
            Thread thread = new Thread(ThreadExec);
            thread.IsBackground = true;
            thread.Start(sShell);

        }

        string sCmdFromat = @"{0} -target:""{1}"" -targetdir:""{2}"" -targetargs:""/site:{3} /config:\""{4}\"""" -register:{5} -output:""{6}""";

        public void Exec(OpenCoverModel.WebModel model, LibClass.OpenCoverModel.GlobalModel globalModel)
        {
            TransfEvent("Web模式，执行站点" + model.Site);
            string sOutput = model.OutPut + "\\" + model.Site;
            XMLEvent(sOutput + "\\" + model.Site + ".xml");
            if (!Directory.Exists(sOutput))
            {
                Directory.CreateDirectory(sOutput);
            }
            string sShell = string.Format(sCmdFromat, globalModel.OpenCover, @"C:\Program Files (x86)\IIS Express\iisexpress.exe", model.TargetDir, model.Site, globalModel.ApplicationHost, globalModel.Register, sOutput + "\\" + model.Site + ".xml");
            Thread thread = new Thread(ThreadExec);
            thread.IsBackground = true;
            thread.Start(sShell);

        }


       public void ThreadExec(object oShell)
        {
            string shell = oShell.ToString();
            TransfEvent("执行脚本：" + shell);

            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = false;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine(shell);
            proc.StandardInput.WriteLine("exit");
            string outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();

            TransfEvent(outStr);

        }

    }
}
