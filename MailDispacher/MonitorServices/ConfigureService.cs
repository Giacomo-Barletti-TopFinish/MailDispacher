using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace MonitorServices
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            var rc = HostFactory.Run(configure =>
            {
                configure.UseLog4Net("..\\..\\App.config");
                HostLogger.Get<Program>().Info("Servizio in fase di avvio");
                configure.Service<WindowsService>(service =>
                {
                    service.ConstructUsing(s => new WindowsService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName("MetalMonitorService");
                configure.SetDisplayName("MetalMonitorService");
                configure.SetDescription("Servizio di monitoring di Metalplus");
                HostLogger.Get<Program>().Info("Servizio avviato");
                configure.StartAutomatically();
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
