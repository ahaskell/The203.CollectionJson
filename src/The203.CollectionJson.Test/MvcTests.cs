using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using The203.CollectionJson.Test.Domain;

namespace The203.CollectionJson.Test
{
    [TestClass]
    [DeploymentItem(@"TestAssets\test.config")]
    public class MvcTests : BaseTestClass
    {
        //   [TestMethod]
        public void SeeWhetherICanMockTheCollectionJsonController()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            request.Setup(r => r.Url).Returns(new Uri("http://test.com/House/Something"));
            request.Setup(r => r.ApplicationPath).Returns("/House/");

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);

            string studyGuideId = Guid.NewGuid().ToString(); // ?? 
            string assessmentItemId = room1.RoomId;


            var service = new Mock<IHouseService>();
            service.Setup(s => s.GetHousesById(studyGuideId)).Returns(sampleHouse);

            RoomDimension emptyRoomDimension = new RoomDimension();

            Mock<Controller> mockController = new Mock<Controller>(service.Object, emptyRoomDimension);

            var controllerContext = new ControllerContext(
                mockHttpContext.Object,
                new RouteData(),
                mockController.Object
                );

            Mock<RequestContext> mockRequestContext = new Mock<RequestContext>();
            mockRequestContext.Setup(rc => rc.HttpContext).Returns(mockHttpContext.Object);
            mockRequestContext.Setup(rc => rc.RouteData).Returns(controllerContext.RouteData);


            mockController.Object.ControllerContext = controllerContext;

            mockController.Object.Url = new UrlHelper(mockRequestContext.Object);

            // Now what?
            //mockController.Object.Get(studyGuideId, assessmentItemId);
        }
    }
}