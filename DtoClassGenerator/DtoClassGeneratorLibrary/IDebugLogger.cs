using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorLibrary
{
    internal interface IDebugLogger
    {
        void PrintDebugInfo(bool isThreadPool = false);
        void ShowErrorMessage(string message);
    }
}
