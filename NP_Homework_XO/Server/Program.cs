using ServerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NP_Homework_XO
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer ws = new WebServer(HttpServerRouter.Router, "http://localhost:8080/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }
    }
}
