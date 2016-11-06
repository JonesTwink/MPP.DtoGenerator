using System;
using System.Collections.Generic;
using System.Threading;

using System.ComponentModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;


namespace DtoClassGeneratorLibrary
{
    public class DtoClassGenerator
    {
        private IDebugLogger consoleWriter = new ConsoleDebugLogger();
        private GeneratorSettings settings = new GeneratorSettings();

        private List<GeneratedClass> generatedClasses;
        private SupportedTypes supportedTypes = new SupportedTypes();

        private object SyncObj = new object();

        public DtoClassGenerator()
        {
            if (settings.MaxThreadsAmount != 0)
            {

            }
        }

        public List<GeneratedClass> Generate(ClassDescription[] classes)
        {
            generatedClasses = new List<GeneratedClass>();
            lock (SyncObj)
            {
                consoleWriter.PrintDebugInfo();
            }
            LaunchClassGenerationThreads(classes);
            
            return generatedClasses;
        }

        private void LaunchClassGenerationThreads(ClassDescription[] classes)
        {
            int classAmount = classes.Length;            

            using (ManualResetEvent waitAllEvent = new ManualResetEvent(false))
            {
                int taskAmount = settings.MaxThreadsAmount;
                ManualResetEvent freeThreadEvent = new ManualResetEvent(true);                    
                foreach (ClassDescription classDescription in classes)
                {
                    freeThreadEvent.WaitOne();
                    if (taskAmount > 0)
                    {
                        taskAmount--;
                        ThreadPool.QueueUserWorkItem(
                        new WaitCallback(x =>
                        {
                            GenerateClass(x);
                            if (Interlocked.Decrement(ref classAmount) == 0)
                            {
                                waitAllEvent.Set();
                            }

                            if (Interlocked.Increment(ref taskAmount) == 1)
                            {
                                freeThreadEvent.Set();
                            }
                               
                        }),
                        classDescription);
                        if (taskAmount == 0)
                            freeThreadEvent.Reset();
                    }
                        
                }
                

                waitAllEvent.WaitOne();
                freeThreadEvent.Close();
            }
        }

        private void GenerateClass(object classDescriptionObj)
        {
            lock (SyncObj)
            {
                consoleWriter.PrintDebugInfo(true);
            }
            
            ClassDescription classDescription = (ClassDescription)classDescriptionObj;
            AdhocWorkspace workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

            List<SyntaxNode> classMembers = new List<SyntaxNode>();
            if (classDescription.Properties != null)
            {
                classMembers = GenerateClassMembers(classDescription.Properties);
            }

            var generatedClassBody = generator.ClassDeclaration(classDescription.ClassName, accessibility: Accessibility.Public,modifiers: DeclarationModifiers.Sealed, members: classMembers);

            var namespaceNode = generator.NamespaceDeclaration(generator.IdentifierName(settings.GeneratedClassNamespace), generatedClassBody);

            var compilationUnitNode = generator.CompilationUnit(namespaceNode);
            compilationUnitNode = Formatter.Format(compilationUnitNode, workspace);

            var generatedClass = new GeneratedClass(classDescription.ClassName, compilationUnitNode.ToString());


            lock(SyncObj)
            {
                generatedClasses.Add(generatedClass);
            }

        }

        public List<SyntaxNode> GenerateClassMembers(PropertyDescription[] classProperties)
        {
            List<SyntaxNode> generatedProperties = new List<SyntaxNode>();

            foreach (PropertyDescription classProperty in classProperties)
            {
                try
                {
                    generatedProperties.Add(GenerateProperty(classProperty));
                }
                catch (DtoClassGeneratorLibraryException ex)
                {
                    consoleWriter.ShowErrorMessage(ex.Message);
                }                
            }
            return generatedProperties;
        }

        private SyntaxNode GenerateProperty(PropertyDescription propertyDescription)
        {
            object type = Type.GetType(propertyDescription.Format);
            try
            {
                var generatedProperty = PropertyDeclaration(IdentifierName(ConvertType(propertyDescription)), propertyDescription.Name)
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                        .WithAccessorList(AccessorList(List(new[] { AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                                                                 AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)) })));
                return generatedProperty;
            }
            catch
            {
                throw new DtoClassGeneratorLibraryException(String.Format("Type {0} is  not supported. Check your input file.", propertyDescription.Format));
            }
            
            
        }

        private string ConvertType(PropertyDescription propertyDescription)
        {        
                if (propertyDescription.Type == "boolean")
                {
                    return supportedTypes["bool"];
                }
                else
                {
                    return supportedTypes[propertyDescription.Format];
                }

        }

      
    }
}