namespace TestsLibrary
{
    public class SourceFile
    {
        public string Path { get; }
        public string Content { get; }

        public SourceFile(string path, string content) {
            Path = path;
            Content = content;
        }
    }
}
