using System.Linq;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Mvc;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class DomainDataCollectionTest
    {
        [TestMethod]
        public void EnsureCollectionJsonRoomDimensionIsBuildingDataCorrectly()
        {
            RouteCollection routes = new RouteCollection();
            RouteBuilder cjLinker = new RouteBuilder();
            cjLinker.StartPrimaryRoute<House>()
                    .AddItemAndCollection<House>("Houses/{houseId}", sg => sg.HouseId)
                    .AddItemAndCollection<House, Room>("Rooms/{roomId}", ai => ai.Id)
                    .AddItemAndCollection<Room, Furniture>("Furniture/{furnitureId}", ans => ans.Id.ToString())
                    .AddItemAndCollection<Room, RoomDimension>("RoomDimension", r => "")
                    .MapRoutes(new RouteCreation(), routes);

            var roomTitle = "good room.";
            var domain = new Room(roomTitle);
            domain.Furniture.Add(new Furniture());
            domain.Furniture.Add(new Furniture());
            domain.Id = "5";
            domain.ClientShallntSeeThis = "4";
            CJ<Room> cjRoomDimension = new CJ<Room>(domain, cjLinker, "");
            cjRoomDimension.BuildLinks().AddParent<House>("parent", new House());
            var actual = from d in cjRoomDimension.CreateCollectionContainer().collection.items.First().data
                         where d.name == "Title"
                         select d.value;
            Assert.IsTrue(actual.Count() == 1);
            Assert.AreEqual(roomTitle, actual.First());
        }

        [TestMethod]
        public void VerifyAddTemplateSetsTemplate()
        {
            RouteCollection routes = new RouteCollection();
            RouteBuilder cjLinker = new RouteBuilder();
            cjLinker.StartPrimaryRoute<House>()
                    .AddItemAndCollection<House>("Houses/{houseId}", sg => sg.HouseId)
                    .AddItemAndCollection<House, Room>("Rooms/{roomId}", ai => ai.Id)
                    .AddItemAndCollection<Room, Furniture>("Furniture/{furnitureId}", ans => ans.Id.ToString())
                    .AddItemAndCollection<Room, RoomDimension>("RoomDimension", r => "")
                    .MapRoutes(new RouteCreation(), routes);

            var expectedMarkup = "good room.";
            var domain = new Room(expectedMarkup);
            domain.Furniture.Add(new Furniture());
            domain.Furniture.Add(new Furniture());
            domain.Id = "5";
            domain.ClientShallntSeeThis = "4";

            CJ<Room> cjRoomDimension = new CJ<Room>(domain, cjLinker, "");
            Assert.IsNull(cjRoomDimension.Container.collection.template);


            cjRoomDimension.AddTemplate();
            Assert.IsNotNull(cjRoomDimension.Container.collection.template);

            // Is it the right template?
            var otherTemplate = (new CollectionJsonTemplating<Room>()).GenerateTemplate();

            Assert.AreEqual(otherTemplate.data.Count, cjRoomDimension.Container.collection.template.data.Count);
            Assert.IsTrue(
                otherTemplate.data.All(
                    d1 => cjRoomDimension.Container.collection.template.data.Any(d => d.name.Equals(d1.name))));
        }
    }
}