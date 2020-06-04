using APBD.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                /*
                1.Metodę HTTP(GET, POST, itd.)
                2.Ścieżkę na którą zostało wysłane żądanie(/ api / students)
                3.Dodanie logowania ciała żądania HTTP(np.wysłany JSON)
                4.Zapisywanie informacji z Query string(?name = Kowalski)
                 */
                string topDivider = "@===============@" +  DateTime.Now + "@===============@";
                string method = httpContext.Request.Method.ToString();
                string path = httpContext.Request.Path; //"weatherforecast /cos"
                string querystring = httpContext.Request?.QueryString.ToString();
                string requestBody = "";
                string bottomDivider = "@=====================================@";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                }

                ArrayList log = new ArrayList();

                log.Add(topDivider);
                log.Add(method);
                log.Add(path);
                log.Add(querystring);
                log.Add(requestBody);
                log.Add(bottomDivider);

                WriteToFIle.Write(log);

                //logowanie do pliku
            }

            await _next(httpContext);
        }


    }
}
