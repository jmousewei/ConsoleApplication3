using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Website.Controllers.ErrorHandling
{
    public class OopsControllerSelector : DefaultHttpControllerSelector
    {
        public OopsControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            return base.SelectController(request);
        }
    }
}