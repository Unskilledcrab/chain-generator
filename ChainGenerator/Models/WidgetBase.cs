using System.Text.RegularExpressions;

namespace ChainGenerator.Models
{
    public class WidgetBase
    {
        public string Title { get; set; }
        public string? Output { get; set; }
    }

    public class WidgetGenerator : WidgetBase
    {
        public string? Prompt { get; set; }
        public string? GeneratedPrompt { get; set; }

        public string? GetFinalPrompt()
        {
            if (ContainsGeneratorReferences())
            {
                return GeneratedPrompt;
            }

            return Prompt;
        }   

        public bool ContainsGeneratorReferences()
        {
            if (Prompt == null)
            {
                return false;
            }

            return Prompt.Contains("{{") && Prompt.Contains("}}");
        }

        // Get everything in the Prompt that begins with {{ and ends with }}
        public string[] GetGeneratorReferences()
        {            
            if (Prompt == null)
            {
                return [];
            }

            var matches = Regex.Matches(Prompt, @"{{(.*?)}}");
            var references = new List<string>();
            foreach (Match match in matches)
            {
                references.Add(match.Groups[1].Value);
            }
            return references.ToArray();
        }
    }

    public class WidgetTextGenerator : WidgetGenerator
    {
        public bool ManualPromptOverride { get; set; } = false;
        public bool IsTextGenerator { get; set; } = true;
        public string? GeneratedOutput { get; set; }
        public string? GeneratedImageUrl { get; set; }

        public string? GetFinalOutput()
        {
            if (ManualPromptOverride)
            {
                return Output;
            }

            return GeneratedOutput;
        }
    }

    public class WidgetImageGenerator : WidgetGenerator
    {
        public string? GeneratedImageUrl { get; set; }
    }
}
