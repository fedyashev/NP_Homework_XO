using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServerLib
{
    public class RouteHandler
    {
        public RouteHandler(string method, string pattern, Action<HttpListenerRequest, HttpListenerResponse, ResponseData, MatchCollection> action)
        {
            Method = method;
            Pattern = pattern;
            Action = action;
        }

        public string Method { get; set; }
        public string Pattern { get; set; }
        public Action<HttpListenerRequest, HttpListenerResponse, ResponseData, MatchCollection> Action { get; set; }
    }
}
