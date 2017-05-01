using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Error
{
    public class Error
    {
        public DateTime Date { get; set; }
        public string ErrorMessage { get; set; }
        public string MethodName { get; set; }
    }
}