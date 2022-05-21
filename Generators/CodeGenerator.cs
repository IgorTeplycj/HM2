using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;


namespace Generators
{
    [Generator]
    public class CodeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

        }

        public void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch();

            var compilation = context.Compilation;
            var imovableInterface = compilation.GetTypeByMetadataName("HM2.GameSolve.Interfaces.IMovable");

            string codeImovableInterface = context.Compilation.SyntaxTrees.Where(x => x.GetText().ToString().
            Contains("interface IMovable")).ToList().First().GetText().ToString();
            var cnt = codeImovableInterface.IndexOf('}');
            string str = codeImovableInterface.Remove(0, codeImovableInterface.LastIndexOf('{') + 1);
            str = str.Remove(str.IndexOf('}'));

            //Получение сигнатуры методов
            List<string> methodsSign = str.Trim(Environment.NewLine.ToCharArray()).Split(Environment.NewLine.ToCharArray()).ToList().
                Where(x => x.Contains(";")).ToList();

            //Создание кода методов
            foreach(var item in methodsSign)
            {
                string s = @$"return HM2.IoCs.IoC<HM2.GameSolve.Structures.Vector>.Resolve(""{item.Trim()}"", )";
            }


            //StringBuilder code = new StringBuilder();

            //context.AddSource("HelloWorldGenerator", SourceText.From(sourceBuilder2.ToString(), Encoding.UTF8));
        }


        string CodeString()
        {
            string code = @"using System;
                            namespace AdapterGenerated
                            {
                                public class HelloWorld
                                {
                                    public string SayHello()
                                    {
                                        return ""Hello from generated code"";
                                    }
                                }
                            }";


            return code;
        }
    }
}
