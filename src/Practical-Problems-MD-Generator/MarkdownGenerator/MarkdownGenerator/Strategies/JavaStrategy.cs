namespace MarkdownGenerator.Strategies
{
    public static class JavaStrategy
    {
        public const string ExecutionStrategy = "java-code";

        public const string Language = "java";

        public const string IdeTemplate =
            "import java.util.Scanner;\r\n\r\npublic class Main {\r\n    public static void main(String[] args) {\r\n        // Write your code here\r\n    }\r\n}";
    }
}