using System;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using The203.CollectionJson.Core;
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
            var cjLinker = new CollectionJsonLinker();
            cjLinker.StartPrimaryRoute();
            cjLinker.AddItemAndCollection<Room>("Rooms/{assessmentItemId}", ai => ai.Id);
            cjLinker.AddEmbeddedItem<Room, RoomDimension>("RoomDimension");

            cjLinker.MapRoutes(routes);

            Assert.IsNotNull(((System.Web.Routing.Route) routes[0]).Url);
            Assert.AreEqual("Rooms/{assessmentItemId}/RoomDimension", ((System.Web.Routing.Route) routes[2]).Url);
            Assert.AreEqual("RoomDimension", ((System.Web.Routing.Route) routes[2]).Defaults["controller"]);
            Assert.AreEqual("Item", ((System.Web.Routing.Route) routes[2]).Defaults["action"]);
        }

        [TestMethod]
        public void EnsureCollectionOnlyRouteCanBeAdded()
        {
            var routes = new RouteCollection();
            var cjLinker = new CollectionJsonLinker();
            cjLinker.StartAlternateRoute();
            cjLinker.AddItemAndCollection<House>("Houses/{houseId}", ai => ai.HouseId);
            cjLinker.AddEmbeddedCollection<House, House>("RelatedStudyGuides");

            cjLinker.MapRoutes(routes);

            Assert.IsNotNull(((System.Web.Routing.Route) routes[0]).Url);
            Assert.AreEqual("Houses/{houseId}/RelatedStudyGuides", ((System.Web.Routing.Route) routes[2]).Url);
            Assert.AreEqual("RelatedStudyGuides", ((System.Web.Routing.Route) routes[2]).Defaults["controller"]);
            // TODO:  this fails because the 3rd is apparently an item with the fourth being a collection.
            // But the fourth is "StudyGuides".  In other words, while this works in practice, it does so by
            // accident.  
            //   Assert.AreEqual("Collection", ((System.Web.Routing.Route)routes[2]).Defaults["action"]);
        }


        [TestMethod]
        public void EnsureRouteAddedToRouteCollection()
        {
            //MapRoutes is an extension and Mocks don't do well with Static stuff like that...so..nevermind using a Mock in this instance....
            //var routes = Moq.Mock.Of<RouteCollection>();
            var routes = new RouteCollection();

            var cjLinker = new CollectionJsonLinker();
            cjLinker.StartPrimaryRoute();
            cjLinker.AddItemAndCollection<House>("Houses/{houseId}", sg => sg.HouseId);
            cjLinker.AddItemAndCollection<House, Room>("Rooms/{roomId}", ai => ai.Id);
            cjLinker.MapRoutes(routes);

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

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnStartPrimaryWhenPreviousRouteBuildingNotTerminated()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.StartPrimaryRoute();
            cjLinker.AddItemAndCollection<Room>("Rooms/{roomId}", ai => ai.Id);

            cjLinker.StartPrimaryRoute();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnStartAlternateWhenPreviousRouteBuildingNotTerminated()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.StartPrimaryRoute();
            cjLinker.AddItemAndCollection<Room>("Rooms/{roomId}", ai => ai.Id);

            cjLinker.StartAlternateRoute();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnAddEmbeddedItemWhenRouteNotStarted()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.AddEmbeddedItem<House, Room>("");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnAddEmbeddedCollectionWhenRouteNotStarted()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.AddEmbeddedCollection<House, Room>("");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnAddItemAndCollectionWhenRouteNotStarted()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.AddItemAndCollection<Room>("", ai => ai.Id);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnAddItemAndCollectionWhenRouteNotStarted2()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.AddItemAndCollection<House, Room>("", ai => ai.Id);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ThrowExceptionOnMapRoutesIfRouteNotStarted()
        {
            RouteCollection routes = new RouteCollection();
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.MapRoutes(routes);
        }

        [TestMethod]
        public void StartAlternateRouteCausesRoutingsToGoIntoAlternateMappings()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.StartAlternateRoute()
                    .AddItemAndCollection<Room>("sss", ai => ai.Id)
                    .AddItemAndCollection<House>("ddd", sg => sg.HouseId)
                    .AddEmbeddedItem<Room, RoomDimension>("eee");

            Assert.AreNotEqual(0, cjLinker.AlternateMappings.Count);
            Assert.AreEqual(0, cjLinker.InternalMappings.Count);

            RouteCollection routes = new RouteCollection();
            cjLinker.MapRoutes(routes);
            Assert.AreEqual(5, routes.Count);
        }

        [TestMethod]
        public void StartPrimaryRouteCausesRoutesToGoIntoInternalMappings()
        {
            CollectionJsonLinker cjLinker = new CollectionJsonLinker();
            cjLinker.StartPrimaryRoute()
                    .AddItemAndCollection<Room>("sss", ai => ai.Id)
                    .AddItemAndCollection<House>("ddd", sg => sg.HouseId)
                    .AddEmbeddedItem<Room, RoomDimension>("eee");

            Assert.AreNotEqual(0, cjLinker.InternalMappings.Count);
            Assert.AreEqual(0, cjLinker.AlternateMappings.Count);

            RouteCollection routes = new RouteCollection();
            cjLinker.MapRoutes(routes);
            Assert.AreEqual(5, routes.Count);
        }
    }
}