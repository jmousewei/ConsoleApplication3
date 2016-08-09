using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Website.Controllers.ErrorHandling
{
    public class OopsExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ExceptionResult(new Exception(context.Exception.ToString()),
                (ApiController) context.ExceptionContext.ControllerContext.Controller);
        }
    }
}