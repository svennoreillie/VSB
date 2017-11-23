using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VSBaseAngular
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                 //.UseHttpSys(options =>
                 //            {
                 //                options.Authentication.Schemes = AuthenticationSchemes.NTLM |
                 //                                                 AuthenticationSchemes.Negotiate;
                 //                options.Authentication.AllowAnonymous = true;
                 //            })
                .Build();
    }
}
