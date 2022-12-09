namespace TestsLibrary
{
    public class TargetFile
    {
        public string SourcePath { get; }
        public string OutputPath { get; }
        public string Content { get; }

        public TargetFile(string sourcePath, string outputPath, string content)
        {
            SourcePath = sourcePath;
            OutputPath = outputPath;
            Content = content;
        }
    }
}
