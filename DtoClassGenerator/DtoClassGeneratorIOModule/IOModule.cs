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
        private IUserMessageLogger messageWriter = new ConsoleUserMessageLogger(); 

        internal IOModule()
        {
            inputFilePath = Directory.GetCurrentDirectory() + "\\input.txt";
            outputDirectoryPath = Directory.GetCurrentDirectory() + "\\Results";
        }

        internal void Listen()
        {
            messageWriter.ShowStartupInfo(inputFilePath, outputDirectoryPath);

            bool isRunning = true;

            while (isRunning)
            {
                try
                {
                    string inputString = Console.ReadLine();
                    isRunning = ProcessInput(inputString);
                }
                catch (IOModuleException ex)
                {
                    messageWriter.ShowErrorMessage(ex.Message);

                }
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
                    messageWriter.ShowMessage(String.Format("Unknown command: {0}", command));
                    break;
            }
            return true;
        }



        private void RunClassGenerator()
        {
            string inputFileContents = ReadFile();

            ClassDescription[] classes = JsonParser.Parse(inputFileContents);

            List<GeneratedClass> generatedClasses = classGenerator.Generate(classes);

            SaveResultsToFile(generatedClasses);
        }

        private void SaveResultsToFile(List<GeneratedClass> generatedClasses)
        {

            foreach (GeneratedClass generatedClass in generatedClasses)
            {

                string filepath = outputDirectoryPath + "\\" + generatedClass.Name + ".cs";
                string contents = generatedClass.Contents;
                try
                {
                    if (!Directory.Exists(outputDirectoryPath))
                    {
                        Directory.CreateDirectory(outputDirectoryPath);
                    }
                    File.WriteAllText(filepath, contents);
                    messageWriter.PrintWriteFileSuccessMessage(filepath);
                }
                catch
                {
                    throw new IOModuleException("An error occured while writing into file. Check your output directory path.");
                }
            }
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
            inputFilePath = inputString.IndexOf(' ') > -1 ? inputString.Substring(inputString.IndexOf(' ')+1, inputString.Length-inputString.IndexOf(' ') - 1) : inputString;
            messageWriter.ShowMessage(String.Format("New input file path has been set to {0}", inputFilePath));
        }

        private void SetOutputDirectoryPath(string inputString)
        {
            outputDirectoryPath = inputString.IndexOf(' ') > -1 ? inputString.Substring(inputString.IndexOf(' ') + 1, inputString.Length - inputString.IndexOf(' ') - 1) : inputString;
            messageWriter.ShowMessage(String.Format("New output directory path has been set to {0}", outputDirectoryPath));
        }
    }
}
