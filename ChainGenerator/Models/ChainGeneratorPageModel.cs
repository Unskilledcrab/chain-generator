using ChainGenerator.Data;

namespace ChainGenerator.Models
{
    public class ChainGeneratorPageModel
    {
        public string Title { get; set; }  // The title of the generator page
        public List<WidgetGeneratorModel> WidgetGeneratorModels { get; set; } = new();
        public ApplicationUser Owner { get; set; }
    }
}
