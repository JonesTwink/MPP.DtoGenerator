using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var newNode = generator.CompilationUnit(generatedClass).NormalizeWhitespace();
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
            return new List<SyntaxNode>();
        }

        public SyntaxNode GenerateProperty(PropertyDescription propertyDescription)
        {
            AdhocWorkspace workspace = new AdhocWorkspace();
            var generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

            var generatedProperty = generator.PropertyDeclaration(propertyDescription.Name,
                                                                      generator.TypeExpression(SpecialType.System_String), Accessibility.Public,
                                                                      getAccessorStatements: new SyntaxNode[]
                                                                      { generator.ReturnStatement(generator.IdentifierName(propertyDescription.Name)) },
                                                                      setAccessorStatements: new SyntaxNode[]
                                                                      { generator.AssignmentStatement(generator.IdentifierName(propertyDescription.Name),
                                                                      generator.IdentifierName("value"))}
                                                                  );

            return generatedProperty;
        }
    }
}