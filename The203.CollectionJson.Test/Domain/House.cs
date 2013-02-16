using System.Collections.Generic;

namespace The203.CollectionJson.Test.Domain
{
    public class House
    {
        public House()
            : this("")
        {
        }

        public House(string houseName)
        {
            HouseId = houseName;
            Rooms = new List<Room>();
        }

        public string HouseId { get; set; }

        public Yard Yard { get; set; }

        public List<Room> Rooms { get; private set; }

        public string OwnerId { get; set; }
    }
}