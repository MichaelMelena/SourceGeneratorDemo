using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceGeneratorLibrary
{
    [Generator]
    public class HelloSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Code generation goes here

            //context.Compilation.SourceModule.ReferencedAssemblies.First(x => x.Name == "OtherLibrary").Name;

            var compilation = context.Compilation.WithReferences(context.Compilation.References.First(x => x.Display == "OtherLibrary"));
            string text = compilation.AssemblyName;

            //var file = Path.GetFileNameWithoutExtension(context.Compilation.SyntaxTrees.First().FilePath);
            //var rootDir = Path.GetDirectoryName("../OtherLibarry");
            //var files = Directory.EnumerateFiles(rootDir, "*.resx", SearchOption.AllDirectories);
            //string text = string.Join("\n", files);
            // Find the main method
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);
            // Build up the source code
            string source = $@" // Auto-generated code
using System;

namespace {mainMethod.ContainingNamespace.ToDisplayString()}
{{
    public static partial class {mainMethod.ContainingType.Name}
    {{
        static partial void HelloFrom(string name)
        {{
            Console.WriteLine(""{text}"");
            Console.WriteLine($""Generator says: Hi from '{{name}}'"");
         
        }}
    }}
}}
";

            var typeName = mainMethod.ContainingType.Name;

            // Add the source code to the compilation
            context.AddSource($"{typeName}.g.cs", source);

        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
            Debug.WriteLine("Initalize code generator");
        }
    }
}
