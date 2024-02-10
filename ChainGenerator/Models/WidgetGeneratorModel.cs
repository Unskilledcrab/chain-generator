using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ChainGenerator.Models
{

    public class WidgetGeneratorModel : BaseEntityModel
    {
        public string Title { get; set; }  // The title of the widget generator

        public string? Prompt { get; set; }  // The prompt for the widget generator, this can contain references to other generators

        public string? PromptIntent { get; set; }

        public string? GeneratedPrompt { get; set; }  // The generated prompt after resolving references to other generators

        public bool IsTextGenerator { get; set; } = true;  // Flag to indicate if the generator is a text generator

        [NotMapped]
        public string? GeneratedOutput
        {
            get
            {
                return GeneratedTextReferences.Count > 0 ? GeneratedTextReferences[SelectedGeneratedTextIndex].Text : null;  // The generated output after the generator has been run
            }
            set
            {
                value = value ?? string.Empty;
                if (GeneratedTextReferences.Count > 0)
                {
                    GeneratedTextReferences[SelectedGeneratedTextIndex].Text = value;
                }
            }
        }

        public string? GeneratedImageUrl => GeneratedImageReferences.Count > 0 ? GeneratedImageReferences[SelectedGeneratedImageIndex].ImageUrl : null;  // The URL of the generated image if the generator is an image generator

        public int SelectedGeneratedTextIndex { get; set; } = 0;

        public List<WidgetTextReferenceModel> GeneratedTextReferences { get; set; } = new List<WidgetTextReferenceModel>();  // List of text references in the generator

        public int SelectedGeneratedImageIndex { get; set; } = 0;

        public List<WidgetImageReferenceModel> GeneratedImageReferences { get; set; } = new List<WidgetImageReferenceModel>();  // List of image references in the generator

        // Method to get the final prompt after resolving references to other generators
        public string? GetFinalPrompt()
        {
            if (ContainsGeneratorTitleReferences())
            {
                return GeneratedPrompt;
            }

            return Prompt;
        }

        public void AddTextReference(string text)
        {
            GeneratedTextReferences.Add(new WidgetTextReferenceModel { Text = text });
            SelectedGeneratedTextIndex = GeneratedTextReferences.Count - 1;
        }

        // Method to check if the prompt contains references to other generators
        public bool ContainsGeneratorTitleReferences()
        {
            if (Prompt == null)
            {
                return false;
            }

            return Prompt.Contains("{{") && Prompt.Contains("}}");
        }

        // Method to get the references to other generators in the prompt
        public string[] GetGeneratorTitleReferences()
        {
            if (Prompt == null)
            {
                return new string[0];
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
}
