using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ReleaseManager.Host.Controllers
{
    public class RootController : ApiController
    {
        private const string Splash = @"<div class=""splash"">
    <div class=""message"">
        Durandal Starter Kit
    </div>
    <i class=""icon-spinner icon-2x icon-spin active""></i>
</div>";

        private const string AppRootHtml = @"<!DOCTYPE html>
<html>
    <head>
        <title>Durandal Starter Kit</title>
        
        <meta charset=""utf-8"" />
        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge, chrome=1"" />
        <meta name=""apple-mobile-web-app-capable"" content=""yes"" />
        <meta name=""apple-mobile-web-app-status-bar-style"" content=""black"" />
        <meta name=""format-detection"" content=""telephone=no""/>
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
        
        <link rel=""apple-touch-startup-image"" href=""~/Content/images/ios-startup-image-landscape.png"" media=""(orientation:landscape)"" />
        <link rel=""apple-touch-startup-image"" href=""~/Content/images/ios-startup-image-portrait.png"" media=""(orientation:portrait)"" />
        <link rel=""apple-touch-icon"" href=""~/Content/images/icon.png""/>
        
        <link href=""/static/Content/ie10mobile.css"" rel=""stylesheet""/>
        <link href=""/static/Content/bootstrap.min.css"" rel=""stylesheet""/>
        <link href=""/static/Content/bootstrap-theme.min.css"" rel=""stylesheet""/>
        <link href=""/static/Content/font-awesome.min.css"" rel=""stylesheet""/>
        <link href=""/static/Content/durandal.css"" rel=""stylesheet""/>
        <link href=""/static/Content/starterkit.css"" rel=""stylesheet""/>
        <link href=""/static/Content/pipelines.css"" rel=""stylesheet""/>
        <link href=""/static/Content/animate.min.css"" rel=""stylesheet""/>

        
        <script type=""text/javascript"">
            if (navigator.userAgent.match(/IEMobile\/10\.0/)) {{
                var msViewportStyle = document.createElement(""style"");
                var mq = ""@@-ms-viewport{{width:auto!important}}"";
                msViewportStyle.appendChild(document.createTextNode(mq));
                document.getElementsByTagName(""head"")[0].appendChild(msViewportStyle);
            }}
        </script>
    </head>
    <body>
        <div id=""applicationHost"">
            <div class=""splash"">
                <div class=""message"">
                    Durandal Starter Kit
                </div>
                <i class=""icon-spinner icon-2x icon-spin active""></i>
            </div>
        </div>
        
        <script src=""/static/Scripts/jquery-2.1.1.js""></script>
        <script src=""/static/Scripts/bootstrap.js""></script>
        <script src=""/static/Scripts/knockout-2.3.0.debug.js""></script>

        <script type=""text/javascript"" src=""/Static/Scripts/require.js"" data-main=""Static/App/main""></script>
    </body>
</html>
";

        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(AppRootHtml, Encoding.UTF8,"text/html");
            return response;
        }
    }
}