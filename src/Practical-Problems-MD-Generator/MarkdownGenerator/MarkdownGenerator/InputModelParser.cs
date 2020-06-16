namespace MarkdownGenerator
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    using Strategies;

    public static class InputModelParser
    {
        public static InputModel GetInputModel(string path)
        {
            var inputContent = File.ReadAllLines(path)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();

            // Problem name
            var problemNameIndex = Array.IndexOf(inputContent, "# Problem Name");
            var problemName = inputContent[problemNameIndex + 1];

            // Task Id
            var taskIdIndex = Array.IndexOf(inputContent, "# Task Id");
            var taskId = inputContent[taskIdIndex + 1];

            // Strategy
            var strategyIndex = Array.IndexOf(inputContent, "# Strategy");
            var language = inputContent[strategyIndex + 1].ToLower();

            var executionStrategy = string.Empty;
            var ideTemplate = string.Empty;

            switch (language)
            {
                case "java":
                    executionStrategy = JavaStrategy.ExecutionStrategy;
                    ideTemplate = JavaStrategy.IdeTemplate;
                    break;
                case "python":
                    executionStrategy = PythonStrategy.ExecutionStrategy;
                    ideTemplate = PythonStrategy.IdeTemplate;
                    break;
                case "html":
                    executionStrategy = HTMLStrategy.ExecutionStrategy;
                    ideTemplate = HTMLStrategy.IdeTemplate;
                    break;
            }

            // Description
            var descriptionIndex = Array.IndexOf(inputContent, "# Description");
            var description = inputContent[descriptionIndex + 1]
                .Replace(
                    ". ",
                    $".{Environment.NewLine}{Environment.NewLine}");

            // Examples
            var examplesRegex = new Regex(@$"## Example{Environment.NewLine}(?<examples>[{Environment.NewLine}\d+\D+]+)");

            var match = examplesRegex.Match(string.Join(Environment.NewLine, inputContent));
            var sb = new StringBuilder();

            if (match.Success)
            {
                var examples = match.Groups["examples"].Value.Split("## Example")
                    .Select(e => e.Split(Environment.NewLine))
                    .ToArray();
                
                foreach (var exampleDetails in examples)
                {
                    var inputs = new List<string>();
                    var outputs = new List<string>();
                    var comments = new List<string>();

                    var isInput = false;
                    var isOutput = false;
                    var isComment = false;
                    
                    foreach (var line in exampleDetails)
                    {
                        if (line == "### Input")
                        {
                            isInput = true;
                            isOutput = false;
                            isComment = false;

                            continue;
                        }

                        if (line == "### Output")
                        {
                            isInput = false;
                            isOutput = true;
                            isComment = false;

                            continue;
                        }

                        if (line == "### Comments")
                        {
                            isInput = false;
                            isOutput = false;
                            isComment = true;

                            continue;
                        }

                        if (isInput)
                        {
                            inputs.Add(line);
                        }
                        else if (isOutput)
                        {
                            outputs.Add(line);
                        }
                        else if (isComment)
                        {
                            comments.Add(line);
                        }
                    }

                    var tableSize = Math.Max(
                        inputs.Count,
                        Math.Max(
                            outputs.Count,
                            comments.Count));

                    for (int i = inputs.Count; i <= tableSize; i++)
                    {
                        inputs.Add(string.Empty);
                    }

                    for (int i = outputs.Count; i <= tableSize; i++)
                    {
                        outputs.Add(string.Empty);
                    }

                    if (comments.Count > 0)
                    {
                        for (int i = comments.Count; i <= tableSize; i++)
                        {
                            comments.Add(string.Empty);
                        }
                    }

                    //| **Input ** | **Output ** |
                    //| --- | --- |
                    //| some title, some content, some author | better title - better content: better author |
                    //| 3 | |
                    //| Edit: better content | |
                    //| ChangeAuthor:  better author | |
                    //| Rename: better title | |

                    if (comments.Count == 0)
                    {
                        sb.AppendLine("| **Input** | **Output** |");
                        sb.AppendLine("| --- | --- |");

                        for (int i = 0; i < tableSize; i++)
                        {
                            var row = $"| {inputs[i]} | {outputs[i]} |";
                            sb.AppendLine(row);
                        }
                    }
                    else
                    {
                        sb.AppendLine("| **Input** | **Output** | **Comments** |");
                        sb.AppendLine("| --- | --- | --- |");

                        for (int i = 0; i < tableSize; i++)
                        {
                            var row = $"| {inputs[i]} | {outputs[i]} | {comments[i]} |";
                            sb.AppendLine(row);
                        }
                    }

                    sb.AppendLine();
                }
            }

            return new InputModel
            {
                ProblemName = problemName,
                TaskId = taskId,
                Description = description,
                Language = language,
                IdeTemplate = ideTemplate,
                ExecutionStrategy = executionStrategy,
                Examples = sb.ToString().TrimEnd()
            };
        }
    }
}