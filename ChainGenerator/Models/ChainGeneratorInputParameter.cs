namespace ChainGenerator.Models
{
    public class ChainGeneratorInputParameter : BaseEntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public ParameterType Type { get; set; }
        public ChainGeneratorPageModel ChainGeneratorPageModel { get; set; }
    }
}
