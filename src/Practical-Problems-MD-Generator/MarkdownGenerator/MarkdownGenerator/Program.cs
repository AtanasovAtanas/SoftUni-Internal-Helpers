namespace MarkdownGenerator
{
    using System;
    using System.IO;

    using Strategies;

    public class Program
    {
        public static void Main()
        {
            var fileName = Console.ReadLine();

            var inputModel = InputModelParser.GetInputModel($"./{fileName}.md");
            var markdownTests = TestsParser.GetTestsInMarkdown($"./{fileName}.zip");

            var template = File.ReadAllText(
                inputModel.Language == HTMLStrategy.Language 
                    ? "./template2.md" 
                    : "./template.md");

            template = template
                .Replace(TemplatePlaceholders.ProblemName, inputModel.ProblemName)
                .Replace(TemplatePlaceholders.TaskId, inputModel.TaskId)
                .Replace(TemplatePlaceholders.ExecutionStrategy, inputModel.ExecutionStrategy)
                .Replace(TemplatePlaceholders.Language, inputModel.Language)
                .Replace(TemplatePlaceholders.IdeTemplate, inputModel.IdeTemplate)
                .Replace(TemplatePlaceholders.Description, inputModel.Description)
                .Replace(TemplatePlaceholders.Examples, inputModel.Examples)
                .Replace(TemplatePlaceholders.Tests, markdownTests);

            Directory.CreateDirectory("./output");

            File.WriteAllText($"./output/{fileName}.md", template);
        }
    }
}
