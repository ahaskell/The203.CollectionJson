using System;
using System.Collections.Generic;
using The203.CollectionJson.Core;

namespace The203.CollectionJson.Test.Domain
{
    public class Room
    {
        public Room()
        {
            Furniture = new List<Furniture>();
        }

        public Room(string p) : this()
        {
            Title = p;
        }

        public String Id { get; set; }
        public string Title { get; private set; }

        [HideFromClient]
        public string QtiAssessmentId { get; set; }

        public string RoomId { get; set; }
        internal RoomDimension RoomDimension { get; set; }

        public IList<Furniture> Furniture { get; private set; }
    }
}