using Microsoft.AspNet.SignalR;
using MIMHubServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core
{
    public static class HubContext
    {
        private static IHubContext _getHubContext;

        public static IHubContext getHubContext
        {
            get
            {
                if (_getHubContext == null)
                {
                    _getHubContext = GlobalHost.ConnectionManager.GetHubContext<MimHubServer>();
                }

                return _getHubContext;
            }
        }

    }
}
