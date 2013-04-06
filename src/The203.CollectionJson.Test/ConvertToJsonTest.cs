using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Core.Model;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class ConvertToJsonTest : BaseTestClass
    {
        [TestMethod]
        public void ValidateInternalDataStructureOfCollectionJsonModel()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Item item = new Item("http://me.com/thing/this/2");
            item.data.Add(new Data("Status", "Answered"));
            item.data.Add(new Data("PaintingsTitle", "Awesomest Painting Ever."));
            item.data.Add(new Data("ItemMarkup", "Epic. Because Epic is an Epic Word, Yo."));
            item.links.Add(new Link("Furniture", "http://server/coco-sg/user/11345/Assessment/2/Question/5/Furniture"));
            item.links.Add(new Link("RoomDimension", "http://server/coco-sg/user/11345/Assessment/2/Question/5/RoomDimension"));
            CollectionContainer container = new CollectionContainer();
            container.collection.href = "http://me.com/thing/this";
            container.collection.items.Add(item);
            string actual = serializer.Serialize(container);
            Assert.IsTrue(actual.Contains("\"collection\":"));
            Assert.IsTrue(actual.Contains("\"items\":"));
            Assert.IsTrue(actual.Contains("\"rel\":\"Furniture\""));
        }

        [TestMethod]
        public void EnsureCollectionJsonRoomDimensionHandlesACollectionCorrectly()
        {
            IList<Room> items = new List<Room>();
            items.Add(room3);
            items.Add(room1);
            items.Add(room2);

            CJ<Room> cjRoomDimension = new CJ<Room>(items, defaultRouteBuilder, "");
            cjRoomDimension.BuildLinks()
                    .AddParent<House>("House", sampleHouse)
                    .AddChildCollection<Furniture>("Furniture", d => d.Furniture)
                    .AddChild<RoomDimension>("RoomDimension", d => d.RoomDimension)
                    .AddSibling<Room>("Next", room2)
                    .AddSibling<Room>("Previous", room2);
            var actual = cjRoomDimension.GenerateJson();
            Assert.AreEqual(3, cjRoomDimension.Container.collection.items.Count());
        }

        [TestMethod]
        public void EnsureCollectionJsonRoomDimensionMakesValidCollectionJson()
        {
            CJ<Room> cjRoomDimension = new CJ<Room>(room3, defaultRouteBuilder, "");
            cjRoomDimension.BuildLinks()
                    .AddParent<House>("House", sampleHouse)
                    .AddChildCollection<Furniture>("Furniture", d => d.Furniture)
                    .AddChild<RoomDimension>("RoomDimension", d => d.RoomDimension)
                    .AddSibling<Room>("Next", room2)
                    .AddSibling<Room>("Previous", room2);
            var actual = cjRoomDimension.GenerateJson();
            Assert.IsTrue(actual.Contains("\"collection\":"));
            Assert.IsTrue(actual.Contains("\"items\":"));
            Assert.IsTrue(actual.Contains("\"rel\":\"Furniture\""));
        }
    }
}