using NUnit.Framework;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using TestsLibrary;
using System.IO;
using System.Linq;

namespace Tests
{
    public class TestsGeneratorTests
    {
        private List<SyntaxNode> roots;
        private List<string> methodList;

        [SetUp]
        public void Setup()
        {
            methodList = new List<string>() { "OverloadedMethod1Test", "OverloadedMethod2Test", "OverloadedMethod3Test", "OverloadedMethod4Test" };
            var path = "../../../../ConsoleApp/NestedClass.cs";
            TargetFile[] testFiles = TestsGenerator.Generate(new SourceFile(path, File.ReadAllText(path)), "../../../generated");
            roots = new List<SyntaxNode>();
            foreach (TargetFile testFile in testFiles)
            {
                roots.Add(CSharpSyntaxTree.ParseText(testFile.Content).GetRoot());
            }
        }

        [Test]
        public void FilesNumTest()
        {
            Assert.That(roots.Count, Is.EqualTo(5));
        }

        [Test]
        public void ClassesTest()
        {
            foreach (var root in roots)
            {
                IEnumerable<ClassDeclarationSyntax> classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                Assert.That(classes.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public void MethodsTest()
        {
            IEnumerable<MethodDeclarationSyntax> methods = roots[0].DescendantNodes().OfType<MethodDeclarationSyntax>();
            Assert.That(methods.Count(), Is.EqualTo(4));
            foreach (MethodDeclarationSyntax method in methods)
            {
                methodList.Remove(method.Identifier.ValueText);
                Assert.Multiple(() =>
                {
                    Assert.That(method.Modifiers.Any(SyntaxKind.PublicKeyword), Is.True);
                    Assert.That(method.ReturnType.ToString(), Is.EqualTo("void"));
                    Assert.That(method.AttributeLists.ToString(), Is.EqualTo("[Test]"));
                    Assert.That(method.Body.Statements.ToString(), Is.EqualTo("Assert.Fail(\"autogenerated\");"));
                });
            }

            Assert.That(methodList.Count, Is.EqualTo(0));
        }

        [Test]
        public void UsingsNumTest()
        {
            IEnumerable<UsingDirectiveSyntax> usings = roots[0].DescendantNodes().OfType<UsingDirectiveSyntax>();
            Assert.That(usings.Count(), Is.EqualTo(9));
        }
    }
}