using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestsLibrary
{
    public class GeneratorPipeline
    {
        private readonly TransformBlock<string, SourceFile> _reader;
        private readonly TransformManyBlock<SourceFile, TargetFile> _generator;
        private readonly ActionBlock<TargetFile> _writer;

        public GeneratorPipeline(string outputPath, GeneratorPipelineConfig config) {
            _reader = new TransformBlock<string, SourceFile>(
                path => ReadFile(path),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = config.ReadParallelism }
            );
            _generator = new TransformManyBlock<SourceFile, TargetFile>(
                sourceFile => GenerateTests(sourceFile, outputPath),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = config.GenerateParallelism }
            );
            _writer = new ActionBlock<TargetFile>(
                testFile => WriteFile(testFile),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = config.WriteParallelism }
            );

            _reader.LinkTo(_generator, new DataflowLinkOptions { PropagateCompletion = true });
            _generator.LinkTo(_writer, new DataflowLinkOptions { PropagateCompletion = true });
        }

        public async Task Generate(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                Console.WriteLine($"{Environment.CurrentManagedThreadId}: Starting file '{file}' processing ...");
                _reader.Post(file);
            }
            await _writer.Completion;
        }

        private static async Task<SourceFile> ReadFile(string sourceFile)
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId}: Reading file '{sourceFile}' ...");
            using (var stream = new StreamReader(sourceFile))
            {
                var contents = await stream.ReadToEndAsync();
                return new SourceFile(sourceFile, contents);
            }
        }

        private static TargetFile[] GenerateTests(SourceFile sourceFile, string outputPath)
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId}: Generating tests for source file '{sourceFile.Path}'");
            return TestsGenerator.Generate(sourceFile, outputPath);
        }

        private static async Task WriteFile(TargetFile testFile)
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId}: Writing test file to '{testFile.OutputPath}' (source '{testFile.SourcePath})' ...");
            using (var streamWriter = new StreamWriter(testFile.OutputPath))
            {
                await streamWriter.WriteAsync(testFile.Content);
            }
        }
    }
}
