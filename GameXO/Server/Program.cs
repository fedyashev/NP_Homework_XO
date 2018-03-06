using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLib;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new WebServer(HttpServerRouter.Router, "http://localhost:8080/");
            app.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            app.Stop();
        }
    }
}
