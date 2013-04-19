using System;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
using The203.CollectionJson.Mvc;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    public class CollectionJsonLinkerTest : BaseTestClass
    {
        [TestMethod]
        public void EnsureItemOnlyRouteCanBeAdded()
        {
            var routes = new RouteCollection();
            var cjLinker = new CollectionJsonLinker()
                .AddItemAndCollection<Room>("Rooms/{assessmentItemId}", ai => ai.Id)
                .AddItemOnly<Room, RoomDimension>("RoomDimension");

            cjLinker.MapRoutes(new RouteCreation(), routes);

            Assert.IsNotNull(((System.Web.Routing.Route) routes[0]).Url);
            Assert.AreEqual("Rooms/{assessmentItemId}/RoomDimension", ((System.Web.Routing.Route) routes[2]).Url);
            Assert.AreEqual("RoomDimension", ((System.Web.Routing.Route) routes[2]).Defaults["controller"]);
            Assert.AreEqual("Item", ((System.Web.Routing.Route) routes[2]).Defaults["action"]);
        }


        [TestMethod]
        public void EnsureRouteAddedToRouteCollection()
        {
            //MapRoutes is an extension and Mocks don't do well with Static stuff like that...so..nevermind using a Mock in this instance....
            //var routes = Moq.Mock.Of<RouteCollection>();
            var routes = new RouteCollection();

            var cjLinker = new CollectionJsonLinker();
            cjLinker.AddItemAndCollection<House>("Houses/{houseId}", sg => sg.HouseId);
            cjLinker.AddItemAndCollection<House, Room>("Rooms/{roomId}", ai => ai.Id);
            cjLinker.MapRoutes(new RouteCreation(), routes);

            Assert.IsNotNull(((System.Web.Routing.Route) routes[0]).Url);
            Assert.AreEqual("Houses/{houseId}", ((System.Web.Routing.Route) routes[0]).Url);
            Assert.AreEqual("Houses", ((System.Web.Routing.Route) routes[0]).Defaults["controller"]);
            Assert.AreEqual("Item", ((System.Web.Routing.Route) routes[0]).Defaults["action"]);
            Assert.AreEqual("Houses/", ((System.Web.Routing.Route) routes[1]).Url);
            Assert.AreEqual("Houses", ((System.Web.Routing.Route) routes[1]).Defaults["controller"]);
            Assert.AreEqual("Collection", ((System.Web.Routing.Route) routes[1]).Defaults["action"]);
            Assert.AreEqual("Houses/{houseId}/Rooms/{roomId}",
                            ((System.Web.Routing.Route) routes[2]).Url);
            Assert.AreEqual("Rooms", ((System.Web.Routing.Route) routes[2]).Defaults["controller"]);
        }
    }
}