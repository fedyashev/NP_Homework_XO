using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppLib
{
    public class HttpServerRouter
    {
        private static List<RouteHandler> routes;

        static HttpServerRouter()
        {
            routes = new List<RouteHandler>()
            {
                new RouteHandler("POST", @"^/api/v1/game/create$", GameCreateHandler),
                new RouteHandler("POST", @"^/api/v1/game/([0-9]+)/join", GameJoinHandler),
                new RouteHandler("GET", @"^/api/v1/game/([0-9]+)/state$", GameStateHandler),
                new RouteHandler("POST", @"^/api/v1/game/([0-9]+)/action$", GameActionHanlder),
                new RouteHandler("GET", @"^/api/v1/game/listeners$", GameListenersHanlder),
            };
        }

        public static void Router(HttpListenerRequest req, HttpListenerResponse res)
        {
            var data = new ResponseData();

            try
            {
                var method = req.HttpMethod.ToUpper();
                var path = req.Url.AbsolutePath;

                var route = routes.Find(p => MatchRoute(req, p.Method, p.Pattern));

                if (route != null)
                {
                    route.Action(req, res, data, Regex.Matches(path, route.Pattern));
                    Console.WriteLine("Route {0}", data.Data);
                }
                else
                {
                    res.StatusCode = 400;
                    data.Data = "Not found";
                }
            }
            catch (Exception)
            {
                res.StatusCode = 500;
                data.Data = "Internal server error";
            }
            finally
            {
                byte[] buf = Encoding.UTF8.GetBytes(data.Data);
                res.ContentLength64 = buf.Length;
                res.OutputStream.Write(buf, 0, buf.Length);
            }

        }

        private static bool MatchRoute(HttpListenerRequest request, string method, string pattern)
        {
            return request.HttpMethod.Equals(method) && Regex.Match(request.Url.AbsolutePath, pattern).Success;
        }

        public static string GetBody(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                throw new ArgumentException();
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            string content = reader.ReadToEnd();
            body.Close();
            reader.Close();
            return content;
        }

        private static void GameActionHanlder(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var id = matches[0].Groups[1].Value;
            data.Data = String.Format("Game action {0}", id);
        }

        private static void GameStateHandler(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var id = matches[0].Groups[1].Value;
            data.Data = String.Format("Game state {0}", id);
        }

        private static void GameCreateHandler(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            //var id = matches[0].Groups[1].Value;
            //data.Data = String.Format("Game create {0}", id);

            var body = GetBody(req);
            Console.WriteLine(body);
            
            data.Data = body;
        }

        private static void GameJoinHandler(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var id = matches[0].Groups[1].Value;
            data.Data = String.Format("Game join {0}", id);
        }

        private static void GameListenersHanlder(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
        
        }
    }
}
