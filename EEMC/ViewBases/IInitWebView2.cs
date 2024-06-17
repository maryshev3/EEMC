using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.ViewBases
{
    public interface IInitWebView2
    {
        async Task InitializeWebView2(WebView2 webView2)
        {
            if (webView2.Source != null)
                return;

            string tempPath = Path.GetTempPath();

            if (String.IsNullOrEmpty(tempPath))
            {
                tempPath = "C:\\Temp";

                if (!Directory.Exists(tempPath)) 
                    Directory.CreateDirectory(tempPath);
            }

            string dirForWebView2 = Path.Combine(tempPath, "EEMC");

            CoreWebView2Environment webView2Environment = await CoreWebView2Environment.CreateAsync(null, dirForWebView2);

            await webView2.EnsureCoreWebView2Async(webView2Environment);
        }
    }
}
