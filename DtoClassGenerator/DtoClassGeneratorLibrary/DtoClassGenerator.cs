using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;

namespace DtoClassGeneratorLibrary
{
    public class DtoClassGenerator
    {
        private List<SyntaxNode> generatedClasses = new List<SyntaxNode>();

        public string Generate(ClassDescription[] classes)
        {

            foreach (ClassDescription classDescription in classes)
            {
                GenerateClass(classDescription);
            }
            AdhocWorkspace workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            foreach (SyntaxNode generatedClass in generatedClasses)
            {
                var newNode = generator.CompilationUnit(generatedClass);
                newNode = Formatter.Format(newNode, workspace);
                Console.WriteLine(newNode.ToString());
            }
            
            return "";
        }

        public void GenerateClass(ClassDescription classDescription)
        {
            AdhocWorkspace workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

            List<SyntaxNode> classMembers = new List<SyntaxNode>();
            if (classDescription.Properties != null)
            {
                classMembers = GenerateClassMembers(classDescription.Properties);                
            }
            var generatedClass = generator.ClassDeclaration(classDescription.ClassName, accessibility: Accessibility.Public, members: classMembers);

            generatedClasses.Add(generatedClass);

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

        public SyntaxNode GenerateProperty(PropertyDescription propertyDescription)
        {
            var generatedProperty = PropertyDeclaration(IdentifierName(propertyDescription.Format), propertyDescription.Name)
                                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                    .WithAccessorList(AccessorList(List( new[] { AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                                                                 AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)) })));

            return generatedProperty;
        }
     
      
    }
}