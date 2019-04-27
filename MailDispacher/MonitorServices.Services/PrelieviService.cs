using MailDispatcher.Common;
using MonitorServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Services
{
    public class PrelieviService
    {
        public void CreaCopiaPrelievi()
        {
            StringBuilder Messaggi = new StringBuilder();
            try
            {
                using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
                {
                    bMagazzino.CopiaPrelievi();
                }
            }
            catch (Exception ex)
            {
                string messaggio = string.Format("ERRORE - in CreaCopiaPrelievi ");
                Messaggi.AppendLine(messaggio);
                messaggio = string.Format("ERRORE - Eccezione {0} ", ex.Message);
                Messaggi.AppendLine(messaggio);

            }
            if (Messaggi.Length > 0)
            {
                string oggetto = string.Format("Errore in crea copia prelievi");
                string corpo = Messaggi.ToString();

                decimal IDMAIL = MailDispatcherService.CreaEmail("VERIFICA REPLICHE", oggetto, corpo);
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void CreaCopiaTrasferimenti()
        {
            StringBuilder Messaggi = new StringBuilder();
            try
            {
                using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
                {
                    bMagazzino.CopiaTrasferimenti();
                }
            }
            catch (Exception ex)
            {
                string messaggio = string.Format("ERRORE - in CreaCopiaTrasferimenti ");
                Messaggi.AppendLine(messaggio);
                messaggio = string.Format("ERRORE - Eccezione {0} ", ex.Message);
                Messaggi.AppendLine(messaggio);

            }
            if (Messaggi.Length > 0)
            {
                string oggetto = string.Format("Errore in crea copia trasferimenti");
                string corpo = Messaggi.ToString();

                decimal IDMAIL = MailDispatcherService.CreaEmail("VERIFICA REPLICHE", oggetto, corpo);
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void CreaCopiaFattibilita()
        {
            StringBuilder Messaggi = new StringBuilder();
            try
            {
                using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
                {
                    bMagazzino.CopiaFattibilita();
                }
            }
            catch (Exception ex)
            {
                string messaggio = string.Format("ERRORE - in CreaCopiaFattibilita ");
                Messaggi.AppendLine(messaggio);
                messaggio = string.Format("ERRORE - Eccezione {0} ", ex.Message);
                Messaggi.AppendLine(messaggio);

            }
            if (Messaggi.Length > 0)
            {
                string oggetto = string.Format("Errore in crea copia fattibilità");
                string corpo = Messaggi.ToString();

                decimal IDMAIL = MailDispatcherService.CreaEmail("VERIFICA REPLICHE", oggetto, corpo);
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }
    }
}
