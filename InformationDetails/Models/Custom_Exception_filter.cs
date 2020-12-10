using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationDetails.Models
{
    public class Custom_Exception_filter:FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if(exceptionContext.Exception is NotImplementedException)
            {
                /////////Write here code
            }
            else if(exceptionContext.Exception is DivideByZeroException)
            {
                ////////Write here code.
            }


            exceptionContext.Result = new ViewResult()
            {
                ViewName= "ExceptionHandler"             // ExceptionHandler this is view name.
            };

            exceptionContext.ExceptionHandled = true;
        }
    }
}