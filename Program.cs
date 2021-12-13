using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using NLog;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Topshelf;

namespace XLWinServiceAPI
{
    class Program
    {
        public static string VersionName = "";
        public static string Version = "";
        
        [STAThread]
        //[UseStaThread]
        static void Main(string[] args)
        {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Version = version;
            VersionName = $"XL WinService API {version} ({name})";


            System.Console.WriteLine($"Listening on http://*:{Properties.Settings.Default.XLSAPI_IPPort}");

            

            HostFactory.Run(x =>
            {
                x.Service<WebService>(s =>
                {
                    s.ConstructUsing(() => new WebService());
                    s.WhenStarted(rs => rs.Start());
                    s.WhenStopped(rs => rs.Stop());
                    s.WhenShutdown(rs => rs.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetServiceName("XLWSAPI");
                x.SetDisplayName("XLWSAPI");
                x.SetDescription("XL WinService API");
            });
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = ConfigureApi();

            const string rootFolder = "./public";
            var fileSystem = new PhysicalFileSystem(rootFolder);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = false,
                FileSystem = fileSystem
            };

            app.UseFileServer(options);

            app.UseWebApi(config);
        }

        private HttpConfiguration ConfigureApi()
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            
            // Replaces the default action invoker to allow actions to be run using an STA thread
            config.Services.Replace(typeof(IHttpActionInvoker), new StaThreadEnabledHttpActionInvoker());

            config.Formatters.Clear();
            config.Formatters.Insert(0, new JsonMediaTypeFormatter());
            config.Formatters.Insert(1, new XmlMediaTypeFormatter());

            config.Services.Replace(typeof(IHttpActionInvoker), new StaThreadEnabledHttpActionInvoker());
            //config.maxre MaxReceivedMessageSize = 2147483647; //2097152



            return config;
        }
    }

    public class WebService
    {
        private IDisposable _app;

        public void Start()
        {
            _app = WebApp.Start<Startup>($"http://+:{Properties.Settings.Default.XLSAPI_IPPort}");
        }

        public void Stop()
        {
            
            if (_app != null)
                _app.Dispose();
        }
    }




}
