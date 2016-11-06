using System;
using System.Collections.Generic;
using System.Threading;
using System.Configuration;
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
        private string generatedClassNamespace;
        private IDebugLogger consoleWriter = new ConsoleDebugLogger();
        private List<GeneratedClass> generatedClasses;
        private SupportedTypes supportedTypes = new SupportedTypes();

        private object SyncObj = new object();

        public DtoClassGenerator()
        {
            int maxThreadCount = 0;
            try
            {
                maxThreadCount = Int32.Parse(ConfigurationManager.AppSettings["maxThreadAmount"]);
                generatedClassNamespace = ConfigurationManager.AppSettings["generatedClassNamespace"];
            }
            catch
            {
                if (generatedClassNamespace == String.Empty)
                {
                    generatedClassNamespace = "DefaultNamespace";
                }
            }
            if (maxThreadCount != 0)
            {
                ThreadPool.SetMinThreads(maxThreadCount, maxThreadCount);
                ThreadPool.SetMaxThreads(maxThreadCount, maxThreadCount);
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
            bool isTheInitialStateSignaling = false;

            using (ManualResetEvent resetEvent = new ManualResetEvent(isTheInitialStateSignaling))
            {
                foreach (ClassDescription classDescription in classes)
                {

                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(x => {
                            GenerateClass(x);
                            if (Interlocked.Decrement(ref classAmount) == 0)
                                resetEvent.Set();
                        }),
                        classDescription);
                }
                resetEvent.WaitOne();
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

            var namespaceNode = generator.NamespaceDeclaration(generator.IdentifierName(generatedClassNamespace), generatedClassBody);

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
                generatedProperties.Add(GenerateProperty(classProperty));
            }
            return generatedProperties;
        }

        private SyntaxNode GenerateProperty(PropertyDescription propertyDescription)
        {
            object type = Type.GetType(propertyDescription.Format);

            var generatedProperty = PropertyDeclaration(IdentifierName(ConvertType(propertyDescription)), propertyDescription.Name)
                                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                    .WithAccessorList(AccessorList(List( new[] { AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                                                                 AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)) })));

            return generatedProperty;
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