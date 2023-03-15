using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVersion02
{
    public class HttpServer
    {
        public static HttpListener _listener;
        public static int Port = 8000;
        public static string urlNumber = "http://127.0.0.1:";

        public static void Start()
        {
            // Create a Http server and start listening for incoming connections
            _listener = new HttpListener();
            _listener.Prefixes.Add(urlNumber + Port.ToString() + "/");
            _listener.Start();
            Console.WriteLine("Listening for connections on {0}", urlNumber);
        }

        public static void Stop()
        {
            _listener.Close();
            Console.WriteLine("Exiting gracefully...");
        }

        public static async Task HandleIncomingConnections(List<string> chartIamgeDev)
        {
            bool runServer = true;

            String pageDataHead =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>Visual page</title>" +
            //chartIamgeDev[0] +
            "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>" +
            "<script src=\"https://cdn.plot.ly/plotly-latest.min.js\"></script>" +
            "<style id=\"plotly.js-style-global\"></style>" +
            "<style id=\"plotly.js-style-modebar-4f4982\"></style>" +

            //"<style id=\"plotly.js-style-global\"></style>"+
            "<style id=\"plotly.js-style-modebar-da7fe5\"></style>" +

            " </head>";

            string pageDataBodyChart =
            "  <body>" +
            "    <p>Page 01 </p>" +
            "    <p>Chart 01</p>" +
            chartIamgeDev[0].ToString() +
            "    <p>Chart 02</p>" +
            chartIamgeDev[1].ToString()
            ;

            string pageDataEnd =
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {0}>" +
            "    </form>" +
            "  </body>" +
            "</html>";

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await _listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                // Write the response info
                string disableSubmit = !runServer ? "disabled" : "";

                // Create an HTML file
                pageDataEnd = string.Format(pageDataEnd, disableSubmit);  // submiting the info
                string pageData = pageDataHead + pageDataBodyChart + pageDataEnd;

                // Convert to binary format
                byte[] data = Encoding.UTF8.GetBytes(pageData); // we transform it int to a byte array
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength; // Set up the website Page's length

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close(); // Close the connection

                // Reference
                // https://codingvision.net/c-simple-http-server
                // https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7
                // https://thoughtbot.com/blog/using-httplistener-to-build-a-http-server-in-csharp
                // https://learn.microsoft.com/en-us/dotnet/api/system.net.httplistener?redirectedfrom=MSDN&view=net-7.0
                // https://stackoverflow.com/questions/21128218/how-to-create-a-simple-local-web-page-using-c-sharp-windows-forms
            }
        }
    }
}