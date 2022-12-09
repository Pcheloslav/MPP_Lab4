using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using TestsLibrary;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sourceFileOption = new Option<string[]>(
                name: "--sourceFile",
                description: "CSharp file to generate test(s) class(-es) for")
                { IsRequired = true };
            var outputPathOption = new Option<string>(
                name: "--outputPath",
                description: "Output path for generated test classes")
                { IsRequired = true };
            var readParallelisimOption = new Option<int>(
                aliases: new string[] { "-r", "--readParallelism" },
                getDefaultValue: () => 1,
                description: "Max degree of parallelism for reading source files");
            var generateParallelisimOption = new Option<int>(
                aliases: new string[] { "-g", "--generateParallelism" },
                getDefaultValue: () => 1,
                description: "Max degree of parallelism for generating tests");
            var writeParallelisimOption = new Option<int>(
                aliases: new string[] { "-w", "--writeParallelism" },
                getDefaultValue: () => 1,
                description: "Max degree of parallelism for writing generated tests");

            var rootCommand = new RootCommand("Tests Generator console App") {
                sourceFileOption,
                outputPathOption,
                readParallelisimOption,
                generateParallelisimOption,
                writeParallelisimOption
            };

            rootCommand.SetHandler(
                async (allSourceFiles, outputPath, readParallelisim, generateParallelisim, writeParallelisim) =>
                {
                    var sourceFiles = allSourceFiles.Distinct();
                    var existingFiles = sourceFiles.Where(sourceFile => File.Exists(sourceFile));

                    // Report missing/non-readable files
                    var missingFiles = sourceFiles.Except(existingFiles);
                    if (missingFiles.Any())
                    {
                        foreach (var missingFile in missingFiles)
                        {
                            Console.Error.WriteLine($"File {missingFile} does not exists or not readable !!!");
                        }
                    }

                    if (!existingFiles.Any())
                    {
                        // Do nothing if there is no files to process
                        Console.WriteLine("No files to process, exiting ...");
                        await new ValueTask();
                    }
                    else
                    {
                        // Ensure output directory exists
                        Directory.CreateDirectory(outputPath);

                        // Generate test files
                        var config = new GeneratorPipelineConfig(readParallelisim, generateParallelisim, writeParallelisim);
                        var pipeline = new GeneratorPipeline(outputPath, config);
                        await pipeline.Generate(sourceFiles);
                    }
                },
                sourceFileOption,
                outputPathOption,
                readParallelisimOption,
                generateParallelisimOption,
                writeParallelisimOption);

            rootCommand.Invoke(args);
        }
    }
}
