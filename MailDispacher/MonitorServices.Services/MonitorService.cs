using MonitorServices.Data;
using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Services
{
    public class MonitorService
    {
        private MonitorDS _ds = new MonitorDS();

        public bool VerificaEsecuzione(string Servizio, out MonitorDS.MONITOR_SCHEDULERRow schedulazione)
        {
            schedulazione = null;

            using (MonitorBusiness bMonitor = new MonitorBusiness())
            {
                _ds.Clear();
                bMonitor.FillMONITOR_SCHEDULER(_ds);

                if (!_ds.MONITOR_SCHEDULER.Any(x => x.SERVIZIO.Trim() == Servizio && x.ESEGUITA == "N" && x.DATAESECUZIONE <= DateTime.Today))
                    return false;

                List<MonitorDS.MONITOR_SCHEDULERRow> schedulazioni = _ds.MONITOR_SCHEDULER.Where(x => x.SERVIZIO == Servizio && x.ESEGUITA == "N" && x.DATAESECUZIONE <= DateTime.Today).ToList();
                foreach (MonitorDS.MONITOR_SCHEDULERRow schedulazioneSelezionata in schedulazioni)
                {
                    if (VerificaOraEsecuzione(schedulazioneSelezionata.ORAESECUZIONE))
                    {
                        schedulazione = schedulazioneSelezionata;
                        return true;
                    }
                }
                return false;
            }
        }

        private bool VerificaOraEsecuzione(string oraEsecuzione)
        {
            string[] str = oraEsecuzione.Split(':');
            if (str.Length != 2)
                return false;

            int ore = int.Parse(str[0]);
            int minuti = int.Parse(str[1]);

            DateTime oraSchedulata = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, ore, minuti, 0);
            if (oraSchedulata < DateTime.Now) return true;
            return false;
        }

        public void AggiornaSchedulazione(MonitorDS.MONITOR_SCHEDULERRow schedulazione)
        {
            schedulazione.ESEGUITA = "S";

            MonitorDS.MONITOR_SCHEDULERRow nuovaSchedulazione = _ds.MONITOR_SCHEDULER.NewMONITOR_SCHEDULERRow();
            nuovaSchedulazione.ESEGUITA = "N";
            nuovaSchedulazione.SERVIZIO = schedulazione.SERVIZIO;
            nuovaSchedulazione.FREQUENZA = schedulazione.FREQUENZA;
            nuovaSchedulazione.ORAESECUZIONE = schedulazione.ORAESECUZIONE;
            switch (nuovaSchedulazione.FREQUENZA)
            {
                case "GIORNALIERA":
                    nuovaSchedulazione.DATAESECUZIONE = DateTime.Today.AddDays(1);
                    break;
            }
            _ds.MONITOR_SCHEDULER.AddMONITOR_SCHEDULERRow(nuovaSchedulazione);

            using (MonitorBusiness bMonitor = new MonitorBusiness())
            {
                bMonitor.UpdateMONITOR_SCHEDULER(_ds);
            }
        }
    }
}
