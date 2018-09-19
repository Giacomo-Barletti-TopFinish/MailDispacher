using Topshelf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailDispacher
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<WindowsService>(service =>
                {
                    service.ConstructUsing(s => new WindowsService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName("MetalMailDispacher");
                configure.SetDisplayName("MetalMailDispacher");
                configure.SetDescription("Servizio email dispacher di Metalplus");
            });
        }
    }
}
