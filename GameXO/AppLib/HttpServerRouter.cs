using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AppLib.Packets;
using AppLib.Entity;
using AppLib.Repository;
using System.IO;

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
                new RouteHandler("GET", @"^.*$", StaticHanlder),
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
                    //Console.WriteLine("Route {0}", data.Data);
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
            var body = request.InputStream;
            var encoding = request.ContentEncoding;
            var reader = new System.IO.StreamReader(body, encoding);
            var content = reader.ReadToEnd();
            body.Close();
            reader.Close();
            return content;
        }

        private static void GameActionHanlder(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var id = matches[0].Groups[1].Value;
            var body = GetBody(req);
            var game = GameRepository
                .GetGames()
                .Find(p => id.Equals(String.Format("{0}", p.Id)));
            if (game != null)
            {
                var reqPacket = JsonConvert.DeserializeObject<ActionPacket>(body);
                if (reqPacket != null)
                {
                    if (game.CompareKey(reqPacket.Key))
                    {
                        if (game.Action(reqPacket.Name, reqPacket.Row, reqPacket.Col))
                        {
                            var resPacket = new ActionSuccessPacket();
                            data.Data = JsonConvert.SerializeObject(resPacket);
                        }
                        else
                        {
                            var resPacket = new ActionFailPacket("Action failed");
                            data.Data = JsonConvert.SerializeObject(resPacket);                            
                        }
                    }
                    else
                    {
                        var resPacket = new ActionFailPacket("Wrong key");
                        data.Data = JsonConvert.SerializeObject(resPacket);
                    }
                }
                else
                {
                    res.StatusCode = 400;
                }
            }
            else
            {
                res.StatusCode = 404;
            }
        }

        private static void GameStateHandler(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var id = matches[0].Groups[1].Value;
            var game = GameRepository
                .GetGames()
                .Find(p => id.Equals(String.Format("{0}", p.Id)));
            if (game != null)
            {
                var packet = new GameStateResponsePacket(game);
                data.Data = JsonConvert.SerializeObject(packet);
            }
            else
            {
                res.StatusCode = 400;
            }
        }

        private static void GameCreateHandler(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var body = GetBody(req);
            var createGamePacket = JsonConvert.DeserializeObject<CreateGamePacket>(body);
            if (createGamePacket != null)
            {
                var game = GameRepository.Create(createGamePacket.Name);
                var successPacket = new CreateGameSuccessPacket(game.Id, game.Key);
                data.Data = JsonConvert.SerializeObject(successPacket);
            }
            else
            {
                throw new Exception("Can't parse packet.");
            }

        }

        private static void GameJoinHandler(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var id = matches[0].Groups[1].Value;
            var body = GetBody(req);
            var reqPacket = JsonConvert.DeserializeObject<JoinPacket>(body);
            if (reqPacket != null)
            {
                var game = GameRepository.GetGames().Find(p => id.Equals(String.Format("{0}", p.Id)));
                if (game != null && !game.Player1Name.Equals(reqPacket.Name))
                {
                    if (game.Join(reqPacket.Name))
                    {
                        var resPacket = new JoinSuccessPacket(game.Key);
                        data.Data = JsonConvert.SerializeObject(resPacket);
                    }
                    else
                    {
                        var resPacket = new JoinFailPacket("Can't join to game.");
                        data.Data = JsonConvert.SerializeObject(resPacket);
                    }
                }
                else
                {
                    res.StatusCode = 400;
                }
            }
            else
            {
                res.StatusCode = 400;
            }
        }

        private static void GameListenersHanlder(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            var list = GameRepository
                .GetGames()
                .FindAll(game => game.isListen())
                .Select(game => new PlayerInfo(game.Id, game.Player1Name))
                .ToList();

            var listenersPacket = new ListenersPacket(list);
            data.Data = JsonConvert.SerializeObject(listenersPacket);
        }

        private static void StaticHanlder(HttpListenerRequest req, HttpListenerResponse res, ResponseData data, MatchCollection matches)
        {
            data.Data = "Static";
            var path = matches[0].Groups[0].Value;
            path = "../../../public/" + path.TrimStart('/');
            Console.WriteLine(path);
            if (File.Exists(path))
            {
                data.Data = File.ReadAllText(path, Encoding.UTF8);
            }
            else
            {
                res.StatusCode = 404;
            }
        }
    }
}
