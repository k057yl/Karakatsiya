namespace Karakatsiya.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResourceKey { get; set; }
        public int SortOrder { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
