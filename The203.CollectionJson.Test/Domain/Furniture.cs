namespace The203.CollectionJson.Test.Domain
{
    public class Furniture
    {
        public Furniture(string p)
        {
            // TODO: Complete member initialization
            Id = p;
        }

        public Furniture()
        {
            // TODO: Complete member initialization
        }

        public string Id { get; set; }

        public int Weight { get; set; }
    }
}