using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace MailDispacher
{
    public class WindowsService
    {
        private LogWriter _log = HostLogger.Get<WindowsService>();

        public void Start()
        {
            try
            {
                _log.Info(String.Format("Connessione al server {0}", string.Empty));

                _log.Info("Orchestratore avviato");

            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di start del servizio", ex);

            }
        }

        public void Stop()
        {
            try
            {

                _log.Info("Orchestratore fermato");
            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di stop del servizio", ex);
            }
        }
    }
}
