using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppLib
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Action<HttpListenerRequest, HttpListenerResponse> _responderMethod;

        public WebServer(string[] prefixes, Action<HttpListenerRequest, HttpListenerResponse> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Action<HttpListenerRequest, HttpListenerResponse> requestHandler, params string[] prefixes)
            : this(prefixes, requestHandler) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                _responderMethod(ctx.Request, ctx.Response);
                                //byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                //ctx.Response.ContentLength64 = buf.Length;
                                //ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch (Exception ex) // suppress any exceptions
                            {
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
