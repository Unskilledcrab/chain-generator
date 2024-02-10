namespace ChainGenerator.Models
{
    public class WidgetImageReferenceModel : BaseEntityModel
    {
        public string? ImageUrl { get; set; }  // The URL of the image
        public WidgetGeneratorModel Widget { get; set; }
    }
}
