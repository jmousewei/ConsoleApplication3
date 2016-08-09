using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Website.Controllers.ErrorHandling
{
    public class OopsExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            //var contentResult = new System.Web.Http.Results.BadRequestErrorMessageResult(context.Exception.ToString(),
            //    (ApiController) context.ExceptionContext.ControllerContext.Controller);
            //context.Result = contentResult;

            if (context.Exception != null)
            {
                throw new Exception("Exception", context.Exception);
            }

            
        }
    }
}