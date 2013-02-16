using System;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class BaseTestClass
    {
        protected WindowTreatment windowTreatment1;
        protected CollectionJsonLinker defaultCollectionJsonLinker;
        protected Lamp lamp1;
        protected Lamp lamp2;
        protected Room room1;
        protected Room room2;
        protected Room room3;
        protected Room room4;
        protected String realRoomTitle = "FrontBackRoom";
        protected House sampleHouse;
        protected Painting painting1;
        protected Painting painting2;

        [TestInitialize]
        public void Setup()
        {
            RouteCollection routes = new RouteCollection();
            defaultCollectionJsonLinker = new CollectionJsonLinker();
            defaultCollectionJsonLinker.StartPrimaryRoute()
                                       .AddItemAndCollection<House>("Houses/{houseId}", sg => sg.HouseId)
                                       .AddItemAndCollection<House, Room>("Rooms/{roomId}", ai => ai.Id)
                                       .AddItemAndCollection<Room, Furniture>("Furniture/{furnitureId}",
                                                                              ans => ans.Id.ToString())
                                       .AddEmbeddedItem<Room, RoomDimension>("RoomDimension")
                                       .MapRoutes(routes);


            painting1 = new Painting("1", "Painting 1", 1);
            painting2 = new Painting("2", "Painting 2", 2);

            lamp1 = new Lamp("1", "Lamp 1", 1);
            lamp2 = new Lamp("2", "Lamp 2", 2);

            windowTreatment1 = new WindowTreatment("WindowTreatments1", "WindowTreatment 1", 2);

            room1 = new Room("Room 1") {Id = "AB"};
            room1.Furniture.Add(new Furniture("Hello Choose me") {Id = "1AB", Weight = 10});
            room1.Furniture.Add(new Furniture("Probably Best to leave me alone") {Id = "2AB", Weight = 0});


            room2 = new Room("Room 2") {Id = "EF"};
            room2.Furniture.Add(new Furniture("Hello Choose me") {Id = "1EF", Weight = 10});
            room2.Furniture.Add(new Furniture("Probably Best to leave me alone") {Id = "2EF", Weight = 0});

            room3 = new Room(realRoomTitle) {Id = "CD"};
            room3.Furniture.Add(new Furniture("Hello Choose me") {Id = "1CD", Weight = 10});
            room3.Furniture.Add(new Furniture("Probably Best to leave me alone") {Id = "2CD", Weight = 0});
            room3.QtiAssessmentId = "4";

            room4 = new Room("Room 4") {Id = "GH"};
            room4.Furniture.Add(new Furniture("Hello Choose Me") {Id = "1GH", Weight = 10});
            room4.Furniture.Add(new Furniture("Probably Best to leave me alone") {Id = "2GH", Weight = 0});

            sampleHouse = new House("123");
        }
    }
}