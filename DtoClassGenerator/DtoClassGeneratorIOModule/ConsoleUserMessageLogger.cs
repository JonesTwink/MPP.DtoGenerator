using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorIOModule
{
    internal class ConsoleUserMessageLogger: IUserMessageLogger
    {
        public void PrintWriteFileSuccessMessage(string filepath)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("A new file has been successfully created.\nPath:");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(filepath);
            Console.ResetColor();
        }

        public void ShowStartupInfo(string inputFilePath,string outputDirectoryPath)
        {
            Console.WriteLine("Input file: {0}", inputFilePath);
            Console.WriteLine("Output directory: {0}", outputDirectoryPath);
            Console.WriteLine("Use commands 'input' and 'output' for setting custom input/output paths.");
        }

        public void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
       
    }
}
