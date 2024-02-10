namespace ChainGenerator.Models
{
    public interface IUpdateable
    {
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }
    }
}
