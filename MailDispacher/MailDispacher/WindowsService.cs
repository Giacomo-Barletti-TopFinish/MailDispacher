using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;
using System.Threading;
using MailDispacher.Properties;
using MailDispatcher.Services;

namespace MailDispacher
{
    public class WindowsService
    {
        private LogWriter _log = HostLogger.Get<WindowsService>();
        private Timer _timer;
        public void Start()
        {
            try
            {
                string messaggio = string.Format("Timer impostato a {0} minuti", Settings.Default.TimerPeriod);
                _timer = new Timer(TimerCallBack, null, 1L, Settings.Default.TimerPeriod * 60 * 1000);
                _log.Info("Servizio Mail Dispatcher avviato");

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
                _timer.Dispose();
                _log.Info("Servizio Mail Dispatcher fermato");
            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di stop del servizio", ex);
            }
        }

        private void TimerCallBack(Object stateInfo)
        {
            try
            {
                MailDispatcherService MDService = new MailDispatcherService();
                MDService.SendMail();
            }
            catch (Exception ex)
            {
                _log.Error("Errore Service Mail Dispatcher", ex);
                while (ex.InnerException != null)
                {
                    _log.Error("--- INNER EXCEPTION", ex);
                    ex = ex.InnerException;
                }
            }
            finally
            {
            }
        }
    }
}
