using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace PokerTable.Web.Tests.Unit
{
    /// <summary>
    /// Mock Context
    /// </summary>
    public class MockContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockContext"/> class.
        /// </summary>
        public MockContext()
        {
            this.RoutingRequestContext = new Mock<RequestContext>(MockBehavior.Loose);
            this.ActionExecuting = new Mock<ActionExecutingContext>(MockBehavior.Loose);
            this.Http = new Mock<HttpContextBase>(MockBehavior.Loose);
            this.Server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            this.Response = new Mock<HttpResponseBase>(MockBehavior.Loose);
            this.Request = new Mock<HttpRequestBase>(MockBehavior.Loose);
            this.Session = new Mock<HttpSessionStateBase>(MockBehavior.Loose);

            this.RoutingRequestContext.SetupGet(c => c.HttpContext).Returns(this.Http.Object);
            this.ActionExecuting.SetupGet(c => c.HttpContext).Returns(this.Http.Object);
            this.Http.Setup(c => c.Request).Returns(this.Request.Object);
            this.Http.Setup(c => c.Response).Returns(this.Response.Object);
            this.Http.Setup(c => c.Server).Returns(this.Server.Object);
            this.Http.Setup(c => c.Session).Returns(this.Session.Object);

            // routing mocks
            this.Request.Setup(r => r.PathInfo).Returns(string.Empty);
            this.Request.Setup(r => r.Path).Returns(string.Empty);
        }

        /// <summary>
        /// Gets the routing request context.
        /// </summary>
        public Mock<RequestContext> RoutingRequestContext { get; private set; }

        /// <summary>
        /// Gets the HTTP.
        /// </summary>
        public Mock<HttpContextBase> Http { get; private set; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public Mock<HttpServerUtilityBase> Server { get; private set; }

        /// <summary>
        /// Gets the response.
        /// </summary>
        public Mock<HttpResponseBase> Response { get; private set; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public Mock<HttpRequestBase> Request { get; private set; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        public Mock<HttpSessionStateBase> Session { get; private set; }

        /// <summary>
        /// Gets the action executing.
        /// </summary>
        public Mock<ActionExecutingContext> ActionExecuting { get; private set; }
    }
}
