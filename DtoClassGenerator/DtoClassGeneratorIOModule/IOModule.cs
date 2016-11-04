using System;
using System.IO;
using Microsoft.CodeAnalysis;
using DtoClassGeneratorLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorIOModule
{
    internal class IOModule
    {
        private string inputFilePath;
        private string outputDirectoryPath;
        private DtoClassGenerator classGenerator = new DtoClassGenerator();

        internal IOModule()
        {
            inputFilePath = Directory.GetCurrentDirectory() + "\\input.txt";
            outputDirectoryPath = Directory.GetCurrentDirectory();
        }

        internal void Listen()
        {
            Console.WriteLine("Input file: {0}", inputFilePath);
            Console.WriteLine("Output directory: {0}", outputDirectoryPath);
            Console.WriteLine("Use commands 'input' and 'output' for setting custom input/output paths.");
            bool isRunning = true;

            while (isRunning)
            {
                string inputString = Console.ReadLine();
                isRunning = ProcessInput(inputString);
            }
        }

        private bool ProcessInput(string inputString)
        {
            string command = ExtractCommand(inputString);

            switch (command)
            {
                case "exit":
                    return false;
                case "input":
                    SetInputFilePath(inputString);
                    
                    break;
                case "output":
                    SetOutputDirectoryPath(inputString);
                    break;
                case "run":
                    RunClassGenerator();
                    break;
                case "":
                    break;
                default:
                    Console.WriteLine("Unknown command: {0}", command);
                    break;
            }
            return true;
        }



        private void RunClassGenerator()
        {
            string inputFileContents = ReadFile();
            ClassDescription[] classes = JsonParser.Parse(inputFileContents);
            classGenerator.Generate(classes);
        }

        private string ReadFile()
        {
            string lines;
            try
            {
                lines = System.IO.File.ReadAllText(inputFilePath);
            }
            catch
            {
                throw new IOModuleException("An error occured while reading from input file. Check your input file path.");
            }

            return lines;
        }

        private string ExtractCommand(string inputString)
        {
            return inputString.IndexOf(' ') > -1 ? inputString.Substring(0, inputString.IndexOf(' ')) : inputString;
        }

        private void SetInputFilePath(string inputString)
        {
            string path = inputString.IndexOf(' ') > -1 ? inputString.Substring(inputString.IndexOf(' ')+1, inputString.Length-inputString.IndexOf(' ') - 1) : inputString;
        }

        private void SetOutputDirectoryPath(string inputString)
        {
            string path = inputString.IndexOf(' ') > -1 ? inputString.Substring(inputString.IndexOf(' ') + 1, inputString.Length - inputString.IndexOf(' ') - 1) : inputString;
        }
    }
}
