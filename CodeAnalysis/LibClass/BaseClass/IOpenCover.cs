using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysis.LibClass.BaseClass
{
    public interface IOpenCover
    {
        void Exec(OpenCoverModel.ExeModel model,LibClass.OpenCoverModel.GlobalModel globalModel);

        void Exec(OpenCoverModel.WebModel model, LibClass.OpenCoverModel.GlobalModel globalModel);
       
    }
}
