using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsLibrary
{
    public class GeneratorPipelineConfig
    {
        public int ReadParallelism { get; }
        public int GenerateParallelism { get; }
        public int WriteParallelism { get; }

        public GeneratorPipelineConfig(int readParallelism, int generateParallelism, int writeParallelism)
        {
            ReadParallelism = readParallelism;
            GenerateParallelism = generateParallelism;
            WriteParallelism = writeParallelism;
        }
    }
}
