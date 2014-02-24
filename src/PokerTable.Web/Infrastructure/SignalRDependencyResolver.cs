using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Castle.Windsor;

namespace PokerTable.Web.Infrastructure
{
    public class SignalRDependencyResolver : Microsoft.AspNet.SignalR.DefaultDependencyResolver
    {
        private readonly IWindsorContainer container;

        public SignalRDependencyResolver(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public override object GetService(Type serviceType)
        {
            return TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return TryGetAll(serviceType).Concat(base.GetServices(serviceType));
        }

        [DebuggerStepThrough]
        private object TryGet(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IEnumerable<object> TryGetAll(Type serviceType)
        {
            try
            {
                var array = container.ResolveAll(serviceType);
                return array.Cast<object>().ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}