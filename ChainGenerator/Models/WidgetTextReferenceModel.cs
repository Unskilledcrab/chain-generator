namespace ChainGenerator.Models
{
    public class WidgetTextReferenceModel : BaseEntityModel
    {
        public string Text { get; set; }  // The text of the reference
        public WidgetGeneratorModel Widget { get; set; }
    }
}
