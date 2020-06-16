namespace MarkdownGenerator
{
    public class InputModel
    {
        public string ProblemName { get; set; }

        public string TaskId { get; set; }

        public string ExecutionStrategy { get; set; }

        public string Language { get; set; }

        public string IdeTemplate { get; set; }

        public string Description { get; set; }

        public string Examples { get; set; }
    }
}