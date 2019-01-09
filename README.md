# Tool
工具类
基于Opencover 分析的C#、.netMvc的百合测试分析，其分析结果为 xml格式，最终使用ReportGenerator 转换成可视化的 html


运行OpenCover需要一系列参数，这里只说明几个主要的参数：

-target：这是目标应用或服务的路径（名称），这里指单元测试工具的路径，支持NUnit和MS Unit

-targetdir：目标目录的路径，如果target argument已经包含了一个路径，那么这个参数可以提供一个查找pdb文件的可选路径

-targetargs：target参数指定的应用所需要的参数(编译测试工程生成的一个dll文件或者EXE文件路径)

-output：输出XML文件的路径，如果没有提供将在当前目录下生成results.xml， 该文件将用于ReportGenerator生成可视化的覆盖率报告

ReportGenerator所需要的参数：

-reports：上述XML文件的路径

-targetdir：生成报告的目录


ReportCreator是报告生成器模板文件的编辑器
