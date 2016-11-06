using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorIOModule
{
    internal interface IUserMessageLogger
    {
        void ShowMessage(string message);
        void ShowErrorMessage(string message);
        void PrintWriteFileSuccessMessage(string filepath);
        void ShowStartupInfo(string inputFilePath, string outputDirectoryPath);
    }
}
