using ChainGenerator.Data;

namespace ChainGenerator.Models
{
    public class ChainGeneratorPageModel : BaseEntityModel, IUpdateable
    {
        public string Title { get; set; }  // The title of the generator page
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }
        public ApplicationUser? Owner { get; set; }
        public List<WidgetGeneratorModel> WidgetGeneratorModels { get; set; } = new();
        public string HtmlOutputTemplate { get; set; }
        public List<ChainGeneratorInputParameter> InputParameters { get; set; } = new();
    }
}
