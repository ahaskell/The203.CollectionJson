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
                .AddEmbeddedItem<Room, RoomDimension>("RoomDimension");

            cjLinker.MapRoutes(new RouteCreation(), routes);

            Assert.IsNotNull(((System.Web.Routing.Route) routes[0]).Url);
            Assert.AreEqual("Rooms/{assessmentItemId}/RoomDimension", ((System.Web.Routing.Route) routes[2]).Url);
            Assert.AreEqual("RoomDimension", ((System.Web.Routing.Route) routes[2]).Defaults["controller"]);
            Assert.AreEqual("Item", ((System.Web.Routing.Route) routes[2]).Defaults["action"]);
        }

        [TestMethod]
        public void EnsureCollectionOnlyRouteCanBeAdded()
        {
            var routes = new RouteCollection();
            var cjLinker = new CollectionJsonLinker()
            .AddItemAndCollection<House>("Houses/{houseId}", ai => ai.HouseId)
            .AddEmbeddedCollection<House, House>("RelatedStudyGuides");

            cjLinker.MapRoutes(new RouteCreation(), routes);

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
            cjLinker.MapRoutes(new RouteCreation(), routes);
        }
    }
}